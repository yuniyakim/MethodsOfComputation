import numpy as np
import matplotlib.pyplot as plt
import sympy as sym
from matplotlib import cm


def grid(a, T, N, M):
    h = a / N
    tau = T / M
    x_p = np.array([i * h for i in range(N + 1)])
    t_p = np.array([i * tau for i in range(N + 1)])
    return x_p, t_p


def conditions(u, a, x, t, kappa):
    f = sym.diff(u, t, 1) - kappa * sym.diff(u, x, 2)
    mu = u.subs(t, 0)
    mu1 = u.subs(x, 0)
    mu2 = u.subs(x, a)
    return f, mu, mu1, mu2


def explicit_scheme(a, T, kappa, N, M, x, t, f, mu, mu1, mu2):
    x_p, t_p = grid(a, T, N, M)
    u_p = np.zeros([N + 1, M + 1], dtype=float)
    h = a / N
    tau = T / M
    for k in range(1, M + 1):
        for i in range(0, N + 1):
            u_p[i, 0] = mu.subs(x, x_p[i])
        for i in range(1, N):
            u_p[i, k] = tau * kappa * (u_p[i + 1, k - 1] - 2 * u_p[i, k - 1] + u_p[i - 1, k - 1]) / (h ** 2) + \
                        tau * f.subs([(x, x_p[i]), (t, t_p[k - 1])]) + u_p[i, k - 1]
        u_p[0, k] = mu1.subs(t, t_p[k])
        u_p[N, k] = mu2.subs(t, t_p[k])
    X, T = np.meshgrid(x_p, t_p, indexing='ij')
    return X, T, u_p


def implicit_scheme(a, T, kappa, N, M, x, t, f, mu, mu1, mu2):
    x_p, t_p = grid(a, T, N, M)
    u_p = np.zeros((N + 1, M + 1), dtype=float)
    A, C = [np.zeros(N + 1, dtype=float) for _ in range(2)]
    B = np.ones(N + 1, dtype=float)
    D = np.zeros(N + 1, dtype=sym.Symbol)
    h, tau = a / N, T / M
    for k in range(1, M + 1):
        D[0] = mu1.subs(t, t_p[k])
        D[N] = mu2.subs(t, t_p[k])
        for i in range(0, N + 1):
            u_p[i, 0] = mu.subs(x, x_p[i])
        for i in range(1, N):
            A[i] = kappa / h ** 2
            B[i] = -2 * kappa / h ** 2 - 1 / tau
            C[i] = kappa / h ** 2
            D[i] = -u_p[i, k - 1] / tau - f.subs([(x, x_p[i]), (t, t_p[k])])
        s = np.zeros(N + 1, dtype=float)
        t1 = np.zeros(N + 1, dtype=float)
        s[0] = -C[0] / B[0]
        t1[0] = D[0] / B[0]
        for i in range(1, N + 1):
            s[i] = -C[i] / (A[i] * s[i - 1] + B[i])
            t1[i] = (D[i] - A[i] * t1[i - 1]) / (A[i] * s[i - 1] + B[i])
        u_p[N, k] = t1[N]
        for i in range(N - 1, -1, -1):
            u_p[i, k] = s[i] * u_p[i + 1, k] + t1[i]
    X, T = np.meshgrid(x_p, t_p, indexing='ij')
    return X, T, u_p


x = sym.symbols('x')
t = sym.symbols('t')

functions = [
    x ** 2 / 4 + t ** 2 / 4,
    t * x,
    x * t ** 3 - 2 * x + 25 - x ** 5
]

a = 10
T = 10
N = 100
M = 100

fig = plt.figure()

for i in range(3):
    kappa = 0.001
    f, mu, mu1, mu2 = conditions(functions[i], a, x, t, kappa)

    ax1 = plt.axes(projection='3d')
    ax1.set_title('Kappa = 0.001')
    x_coord, t_coord, result = explicit_scheme(a, T, kappa, N, M, x, t, f, mu, mu1, mu2)
    surf = ax1.plot_surface(x_coord, t_coord, result, cmap=cm.hot, linewidth=0, antialiased=False)
    fig.colorbar(surf, shrink=0.5, aspect=5)
    fig.savefig(str(i + 1) + '.1.png')
    plt.clf()

    ax2 = plt.axes(projection='3d')
    ax2.set_title('Kappa = 0.001')
    x_coord, t_coord, result = implicit_scheme(a, T, kappa, N, M, x, t, f, mu, mu1, mu2)
    surf = ax2.plot_surface(x_coord, t_coord, result, cmap=cm.hot, linewidth=0, antialiased=False)
    fig.colorbar(surf, shrink=0.5, aspect=5)
    fig.savefig(str(i + 1) + '.2.png')
    plt.clf()

    kappa = 0.1

    ax3 = plt.axes(projection='3d')
    ax3.set_title('Kappa = 0.1')
    x_coord, t_coord, result = explicit_scheme(a, T, kappa, N, M, x, t, f, mu, mu1, mu2)
    surf = ax3.plot_surface(x_coord, t_coord, result, cmap=cm.hot, linewidth=0, antialiased=False)
    fig.colorbar(surf, shrink=0.5, aspect=5)
    fig.savefig(str(i + 1) + '.3.png')
