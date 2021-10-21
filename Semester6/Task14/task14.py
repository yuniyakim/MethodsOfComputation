import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from numpy.linalg import norm
from inspect import getfullargspec
from tabulate import tabulate


def calculate_gradient(u, x0, dx=1e-7):
    n = len(x0)
    gradient = np.zeros(n)
    for i in range(n):
        delta = np.array([0 if j != i else dx for j in range(n)])
        gradient[i] = (u(*(x0 + delta)) - u(*x0)) / dx
    return gradient


def gradient_descent(function, learning_rate, epsilon, dx=1e-7):
    max_iterations = 2000
    amount_of_iterations = 0
    n = len(getfullargspec(function)[0])
    teta0 = np.array([np.random.normal() for _ in range(n)])
    teta1 = teta0 - learning_rate * calculate_gradient(function, teta0, dx)
    while norm(teta0 - teta1) > epsilon and amount_of_iterations < max_iterations:
        amount_of_iterations += 1
        teta0 = teta1
        teta1 = teta0 - learning_rate * calculate_gradient(function, teta0, dx)
    return teta0, amount_of_iterations


def nesterov_method(function, epsilon):
    n = len(getfullargspec(function)[0])
    y1 = np.array([np.random.normal() for _ in range(n)])
    z = np.array([np.random.normal() for _ in range(n)])
    k = 0
    a1 = 1
    x0 = y1
    a0 = norm(y1 - z) / norm(calculate_gradient(function, y1) - calculate_gradient(function, z))
    i = 0
    while function(*y1) - function(*(y1 - 2 ** (-i) * a0 * calculate_gradient(function, y1))) < 2 ** (-i - 1) * a0 * (
            norm(calculate_gradient(function, y1)) ** 2):
        i += 1
    a1 = 2 ** (-i) * a0
    x1 = y1 - a1 * calculate_gradient(function, y1)
    a2 = (1 + (4 * a1 ** 2 + 1) ** 0.5) / 2
    y2 = x1 + ((a1 - 1) * (x1 - x0)) / a2
    while norm(y1 - y2) > epsilon and k < 2000:
        k += 1
        a0, a1, x0, y1 = a1, a2, x1, y2
        i = 0
        while function(*y1) - function(*(y1 - 2 ** (-i) * a0 * calculate_gradient(function, y1))) < 2 ** (-i - 1) * a0 * (
                norm(calculate_gradient(function, y1)) ** 2):
            i += 1
        a1 = 2 ** (-i) * a0
        x1 = y1 - a1 * calculate_gradient(function, y1)
        a2 = (1 + (4 * a1 ** 2 + 1) ** 0.5) / 2
        y2 = x1 + ((a1 - 1) * (x1 - x0)) / a2
    return y2, k


def g_function(x):
    return 4 * x + 11


def loss_function(X, Y):
    return lambda a, b: np.sum(np.array([0.5 * (a * x + b - y) ** 2 for x, y in zip(X, Y)]))

N = 100
a, b = -10, 10
h = (b - a) / N
X = np.array([a + i * h for i in range(N)])
Y = np.array([g_function(x) for x in X])

f = loss_function(X, Y)

table_gradient_descent = pd.DataFrame(columns=["Lambda", "Epsilon", "Amount of iterations", "|a_ε-a|", "|b_ε-b|", "Loss"])
for i in range(2, 7, 2):
    for j in range(2, 7, 2):
        my_lambda, epsilon = 10 ** (-i), 10 ** (-j)
        result, amount_of_iterations = gradient_descent(f, my_lambda, epsilon)
        row = [my_lambda, epsilon, amount_of_iterations, abs(result[0] - 4), abs(result[1] - 11), f(*result)]
        table_gradient_descent = table_gradient_descent.append(pd.Series(row, index=table_gradient_descent.columns), True)
pd.set_option('display.max_columns', None)
pd.set_option('display.expand_frame_repr', False)
print(tabulate(table_gradient_descent, headers='keys', tablefmt='github', showindex=False))

print()
print()

table_nesterov_method = pd.DataFrame(columns=["Epsilon", "Amount of iterations", "|a_ε-a|", "|b_ε-b|", "Loss"])
for j in range(2, 9, 2):
    epsilon = 10 ** (-j)
    result, amount_of_iterations = nesterov_method(f, epsilon)
    row = [epsilon, amount_of_iterations, abs(result[0] - 4), abs(result[1] - 11), f(*result)]
    table_nesterov_method = table_nesterov_method.append(pd.Series(row, index=table_nesterov_method.columns), True)
print(tabulate(table_nesterov_method, headers='keys', tablefmt='github', showindex=False))


Y1 = np.array([g_function(x) + np.random.normal(0, 8, 1) for x in X])
plt.scatter(X, Y1, color="red")
plt.plot(X, Y, color="blue")

f1 = loss_function(X, Y)

fig11, axes11 = plt.subplots(1, 3, figsize=(20, 6))
fig12, axes12 = plt.subplots(1, 3, figsize=(20, 6))
fig13, axes13 = plt.subplots(1, 3, figsize=(20, 6))
for i in range(3):
    for j in range(3):
        my_lambda, epsilon = 10 ** (-((i + 1) * 2)), 10 ** (-((j + 1) * 2))
        result, amount_of_iterations = gradient_descent(f1, my_lambda, epsilon)
        if i == 0:
            axes11[j].scatter(X, Y1, color="red")
            axes11[j].plot(X, np.array([result[0] * x + result[1] for x in X]), color="blue")
            axes11[j].set_title("Epsilon: {}, lambda: {}. \n Iterations: {}, loss: {}.".format(epsilon, my_lambda, amount_of_iterations, f1(*result)))
        elif i == 1:
            axes12[j].scatter(X, Y1, color="red")
            axes12[j].plot(X, np.array([result[0] * x + result[1] for x in X]), color="blue")
            axes12[j].set_title("Epsilon: {}, lambda: {}. \n Iterations: {}, loss: {}.".format(epsilon, my_lambda, amount_of_iterations, f1(*result)))
        else:
            axes13[j].scatter(X, Y1, color="red")
            axes13[j].plot(X, np.array([result[0] * x + result[1] for x in X]), color="blue")
            axes13[j].set_title("Epsilon: {}, lambda: {}. \n Iterations: {}, loss: {}.".format(epsilon, my_lambda, amount_of_iterations, f1(*result)))
fig11.savefig("1.1.png")
fig12.savefig("1.2.png")
fig13.savefig("1.3.png")


fig21, axes21 = plt.subplots(1, 2, figsize=(20, 8))
fig22, axes22 = plt.subplots(1, 2, figsize=(20, 8))
for i in range(4):
    epsilon = 10 ** (-((i + 1) * 2))
    result, amount_of_iterations = nesterov_method(f1, epsilon)
    if i == 0 or i == 1:
        axes21[i].scatter(X, Y1, color="red")
        axes21[i].plot(X, np.array([result[0] * x + result[1] for x in X]), color="blue")
        axes21[i].set_title("Epsilon: {}, iterations: {}. \n Loss: {}".format(epsilon, amount_of_iterations, f1(*result)))
    else:
        axes22[i - 2].scatter(X, Y1, color="red")
        axes22[i - 2].plot(X, np.array([result[0] * x + result[1] for x in X]), color="blue")
        axes22[i - 2].set_title("Epsilon: {}, iterations: {}. \n Loss: {}".format(epsilon, amount_of_iterations, f1(*result)))
fig21.savefig("2.1.png")
fig22.savefig("2.2.png")
