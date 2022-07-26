using System;
using System.Collections.Generic;
using UnityEngine;

public class MatrizDiscreta : IMatriz
{
    private static float epsilon = 0.001f;

    private Dictionary<uint, float> _matriz;
    private uint _tamanioX, _tamanioY;

    public MatrizDiscreta(uint tamanioX, uint tamanioY)
    {
        _matriz = new Dictionary<uint, float>();
        _tamanioX = tamanioX;
        _tamanioY = tamanioY;
    }

    public float this[uint i, uint j]
    {
        get
        {
            uint posicion = i * _tamanioX + j;
            return _matriz.ContainsKey(posicion) ? _matriz[posicion] : 0;
        }
        set
        {
            uint posicion = i * _tamanioX + j;
            if (_matriz.ContainsKey(posicion))
                _matriz[posicion] = value;
            else
                _matriz.Add(posicion, value);            
        }
    }

    public bool EsCuadrada() => _tamanioX == _tamanioY;
    public bool SonMultiplicables(IMatriz otro) => _tamanioY == otro.Tamanio().Item1;

    public Tuple<uint, uint> Tamanio() => new Tuple<uint, uint>(_tamanioX, _tamanioY);

    public IMatriz Transponer()
    {
        MatrizDiscreta matrizTranspuesta = new MatrizDiscreta(_tamanioY, _tamanioX);

        foreach (KeyValuePair<uint, float> keyValue in _matriz)
        {
            uint i = keyValue.Key / _tamanioX;
            uint j = keyValue.Key % _tamanioX;

            matrizTranspuesta[j, i] = keyValue.Value;
        }

        return matrizTranspuesta;
    }

    public IMatriz Multiplicar(IMatriz otro)
    {
        if (!SonMultiplicables(otro))
            return null;

        uint tamanioFinalX = _tamanioX, tamanioFinalY = otro.Tamanio().Item2;
        Matriz resultado = new Matriz(tamanioFinalX, tamanioFinalY);

        uint cantidadIguales = _tamanioY;
        for (uint i = 0; i < tamanioFinalX; i++)
            for (uint j = 0; j < tamanioFinalY; j++)
            {
                for (uint k = 0; k < cantidadIguales; k++)
                    resultado[i, j] += this[i, k] * otro[k, j];
            }

        return resultado;
    }

    public IMatriz Sumar(IMatriz otro)
    {
        if (!SonDelMismoTamanio(otro))
            return null;

        Matriz resultado = new Matriz(_tamanioX, _tamanioY);

        for (uint i = 0; i < _tamanioX; i++)
            for (uint j = 0; j < _tamanioY; j++)
                resultado[i, j] = otro[i, j];

        foreach (KeyValuePair<uint, float> keyValue in _matriz)
        {
            uint i = keyValue.Key / _tamanioX;
            uint j = keyValue.Key % _tamanioX;

            resultado[i, j] += keyValue.Value;
        }

        return resultado;
    }

    private bool SonDelMismoTamanio(IMatriz otro)
    {
        return Tamanio().Item1 == otro.Tamanio().Item1 && Tamanio().Item2 == Tamanio().Item2;
    }

    public IMatriz Restar(IMatriz otro)
    {
        return Sumar(otro.Multiplicar(-1));
    }

    public IMatriz Multiplicar(float valor)
    {
        MatrizDiscreta resultado = new MatrizDiscreta(_tamanioX, _tamanioY);

        foreach (KeyValuePair<uint, float> keyValue in _matriz)
        {
            uint i = keyValue.Key / _tamanioX;
            uint j = keyValue.Key % _tamanioX;

            resultado[i, j] = keyValue.Value * valor;
        }

        return resultado;
    }

    public float ModuloInfinito()
    {
        Vector resultado = new Vector(_tamanioX);
        foreach (KeyValuePair<uint, float> keyValue in _matriz)
        {
            uint i = keyValue.Key / _tamanioX;
            resultado[i] += Mathf.Abs(keyValue.Value);
        }
        return resultado.ModuloInfinito();
    }

    public bool EsSimetrica()
    {
        if (!EsCuadrada())
            return false;

        bool esSimetrica = true;
        foreach (KeyValuePair<uint, float> keyValue in _matriz)
        {
            uint i = keyValue.Key / _tamanioX;
            uint j = keyValue.Key % _tamanioX;

            esSimetrica &= (keyValue.Value < this[j, i] + epsilon && keyValue.Value > this[j, i] - epsilon);
        }

        return esSimetrica;
    }

    public bool EsDiagonalmenteDominante()
    {
        if (!EsCuadrada())
            return false;

        bool esDiagonalmenteDominante = true;
        for (uint i = 0; i < _tamanioX && esDiagonalmenteDominante; i++)
        {
            float valor = 0;
            for (uint j = 0; j < _tamanioY && esDiagonalmenteDominante; j++)
                valor += (i == j) ? 0 : Mathf.Abs(this[i, j]);
            esDiagonalmenteDominante &= Mathf.Abs(this[i, i]) > valor;
        }

        return esDiagonalmenteDominante;
    }
}
