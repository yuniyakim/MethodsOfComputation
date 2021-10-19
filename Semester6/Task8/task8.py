import numpy as np
import matplotlib.pyplot as plt
from scipy.special import eval_jacobi
from scipy.misc import derivative
from scipy.integrate import quad
from numpy.linalg import solve
from math import exp, cos, log


def jacobi_polynomial(n, k):
    return lambda t: (1 - t ** 2) * eval_jacobi(n, k, k, t)


def jacobi_polynomial_first_derivative(n, k):
    return lambda t: derivative(jacobi_polynomial(n, k), t)


def jacobi_polynomial_second_derivative(n, k):
    return lambda t: derivative(jacobi_polynomial_first_derivative(n, k), t)


def a_function(functions, phi, dphi, ddphi, i):
    k, p, q, f = functions
    return lambda x: k(x) * ddphi[i](x) + p(x) * dphi[i](x) + q(x) * phi[i](x)


def galerkin_method(segment, functions, N):
    a, b = segment
    k, p, q, f = functions
    phi = [jacobi_polynomial(i, 1) for i in range(N)]
    dphi = [jacobi_polynomial_first_derivative(i, 1) for i in range(N)]
    ddphi = [jacobi_polynomial_second_derivative(i, 1) for i in range(N)]
    A = np.array([a_function(functions, phi, dphi, ddphi, i) for i in range(N)])
    C = np.array([quad(lambda t: f(t) * phi[i](t), a, b)[0] for i in range(N)])
    B = np.zeros([N, N])
    for i in range(N):
        for j in range(N):
            B[i, j] = quad(lambda t: phi[i](t) * A[j](t), a, b)[0]
    alpha = solve(B, C)
    return lambda t: sum([alpha[i] * phi[i](t) for i in range(N)])


functions = [
    [
        lambda x: -(4 + x) / (5 + 2 * x),
        lambda x: x / 2 - 1,
        lambda x: 1 + exp(x / 2),
        lambda x: 2 + x
    ],
    [
        lambda x: -(4 - x) / (5 - 2 * x),
        lambda x: (1 - x) / 2,
        lambda x: log(3 + x) / 2,
        lambda x: 1 + x / 3
    ],
    [
        lambda x: -(6 + x) / (7 + 3 * x),
        lambda x: -(1 - x / 2),
        lambda x: 1 + cos(x) / 2,
        lambda x: 1 - x / 3
    ]]

segments = [[-1, 1], [-1, 1], [-1, 1]]

for i in range(3):
    for j in range(3):
        if j == 0:
            N = 3
            h = 0.05
        elif j == 1:
            N = 7
            h = 0.03
        else:
            N = 11
            h = 0.01
        u = galerkin_method(segments[i], functions[i], N)
        a, b = segments[i]
        n = round((b - a) / h)
        x1 = np.zeros(n + 1)
        y = np.zeros(n + 1)
        for t in range(n + 1):
            x1[t] = a + t * h
            y[t] = u(x1[t])
        plt.plot(x1, y, marker='.', mec='red', ms=3)
        plt.title("N = {}".format(N - 1))
        plt.savefig(str(i + 1) + "." + str(j + 1) + ".png")
        plt.clf()