﻿using ShareKernel.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vuelos.Domain.Event;
using Vuelos.Domain.Model.Aeronaves.ValueObjects;
using Vuelos.Domain.ValueObjects;


namespace Vuelos.Domain.Model.Aeronaves
{
    public class Aeronave : AggregateRoot<Guid>
    {
        // !!! completar Value Objects
        public string NroAeronave { get; private set; }
        public MatriculaValue Matricula { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public CapacidadAsientosValue CapacidadAsientos { get; private set; }
        public int CapacidadCombustible { get; private set; }
        public bool EsActivo { get; private set; }
        private readonly ICollection<AsientosAeronave> _detalleAsientos;


        public IReadOnlyCollection<AsientosAeronave> DetalleAsientos
        {
            get
            {
                return new ReadOnlyCollection<AsientosAeronave>(_detalleAsientos.ToList());
            }
        }


        private Aeronave() { }


        internal Aeronave(string nroAeronave)   // !!! ver. si añadir los demás campos
        {
            Id = Guid.NewGuid();
            NroAeronave = nroAeronave;
            EsActivo = true;
            _detalleAsientos = new List<AsientosAeronave>();

        }


        public void AgregarItem(NroAsientoValue nroAsiento, FilaAsientoValue fila, ColumnaAsientoValue columna,
                    EstadoAsientoValue estadoAsiento)            
        {
            var DetAsientos = new AsientosAeronave(nroAsiento, fila, columna, estadoAsiento);
            _detalleAsientos.Add(DetAsientos);
            //var detallePedido = _detalle.FirstOrDefault(x => x.ProductoId == productoId);
            //if (detallePedido is null)
            //{
            //    detallePedido = new DetallePedido(productoId, instrucciones, cantidad, precio);
            //    _detalle.Add(detallePedido);
            //}
            //else
            //{
            //    detallePedido.ModificarPedido(cantidad, precio);  !!! Ver. si va implementar modificar
            //}
            AddDomainEvent(new AsientoAeronaveAgregada(Id, nroAsiento, estadoAsiento)); // !!! va el Id de la Aeronave
        }


        public void ConsolidarAeronave()
        {
            var evento = new AeronaveCreada(Id, Matricula);
            AddDomainEvent(evento);
        }
    }
}
