using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ItIsNotOnlyMe.LinealSolver;
using UnityEngine.TestTools.Utils;

public class LinealSolverTest
{
    IMatriz _matrizA, _vectorB;
    float _errorMaxima;
    int _cantidadIteraciones;
    uint _tamanio;

    List<float> _resultado;

    public LinealSolverTest()
    {
        _tamanio = 5;
        _matrizA = new Matriz(_tamanio, _tamanio);
        _vectorB = new Vector(_tamanio);

        List<float> valoresA = new List<float>
        {
            25, 2, 5, 7, 7,
            2, -15, 2, 2, 4,
            5, 2, 30, 0, 3,
            7, 2, 0, 20, 7,
            7, 4, 3, 7, 30
        };
        List<float> valoresB = new List<float>
        {
            9, 8, 9, 5, 0
        };

        for (uint i = 0; i < _tamanio; i++)
        {
            for (uint j = 0; j < _tamanio; j++)
            {
                uint posicion = i * _tamanio + j;
                _matrizA[i, j] = valoresA[(int)posicion];
            }
            _vectorB[i, 0] = valoresB[(int)i];
        }

        _errorMaxima = 0.0001f;
        _cantidadIteraciones = 20;

        _resultado = new List<float>
        {
            0.301742f,
            -0.449369f,
            0.288765f,
            0.221168f,
            -0.090973f,
        };
    }

    [Test]
    public void Test01JacobiResuelveCorrectamenteUnSistemaDeEcuaciones()
    {
        IMatriz resultado = LinealSolver.Jacobi(_matrizA, _vectorB, _cantidadIteraciones, _errorMaxima);

        Assert.AreNotEqual(resultado, null);
        var comparer = new FloatEqualityComparer(_errorMaxima);

        for (uint i = 0; i < _tamanio; i++)
            Assert.That(resultado[i, 0], Is.EqualTo(_resultado[(int)i]).Using(comparer));
    }

    [Test]
    public void Test02GaussSeidelResuelveCorrectamenteUnSistemaDeEcuaciones()
    {
        IMatriz resultado = LinealSolver.GaussSeidel(_matrizA, _vectorB, _cantidadIteraciones, _errorMaxima);

        Assert.AreNotEqual(resultado, null);
        var comparer = new FloatEqualityComparer(0.1f);

        for (uint i = 0; i < _tamanio; i++)
            Assert.That(resultado[i, 0], Is.EqualTo(_resultado[(int)i]).Using(comparer));
    }

    [Test]
    public void Test03GradienteConjugadoResuelveCorrectamenteUnSistemaDeEcuaciones()
    {
        IMatriz resultado = LinealSolver.GradienteConjugado(_matrizA, _vectorB, _cantidadIteraciones, _errorMaxima);

        Assert.AreNotEqual(resultado, null);
        var comparer = new FloatEqualityComparer(_errorMaxima);

        for (uint i = 0; i < _tamanio; i++)
            Assert.That(resultado[i, 0], Is.EqualTo(_resultado[(int)i]).Using(comparer));
    }
}
