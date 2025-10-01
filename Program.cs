//Nombre: Christopher Aguilar O.

// Fecha: 26/09/2024

/* Descripción: Programa que calcula el aumento salariales de una empresa, teniendo 3 opciones de empleado se utiliza un menú para agregar varios empleados.

 * El sistema como primera instancia solicita la cédula del empleado, el nombre y luego el tipo de empleado.

 * empleado es tipo 1 (Operario) el aumento es de un 15% (0,15)  sobre el salario ordinario actual.

 * empleado es tipo 2 (Técnico)    el aumento es de un 10%  (0.10) sobre el salario ordinario actual.

 * empleado es tipo 3 (Profesional)    el aumento es de un 5%  (0.05) sobre el salario ordinario

 * Por Ultimo se debe mostrar el salario actual, el aumento y el nuevo salario y además mostrar las deducciones.

 * Cuando el usuario quiera finalizar el programa, se debe mostrar lo siguiente:

 * 1)  Cantidad Empleados Tipo Operarios

2)  Acumulado Salario Neto para Operarios

3) Promedio Salario Neto para Operarios

4)  Cantidad Empleados Tipo Técnico

5)  Acumulado Salario Neto para Técnicos

6) Promedio Salario Neto para Técnicos

7) Cantidad Empleados Tipo Profesional

8)  Acumulado Salario Neto para Profesional

9) Promedio Salario Neto para Profesionales

 */

using System;

namespace AumentoSalarioApp
{
    class Program
    {
        // Constantes
        private const double DEDUCCION_PORCENTAJE = 9.17;
        private const int OPCION_AGREGAR_EMPLEADO = 1;
        private const int OPCION_SALIR = 2;

        // Tipos de empleado
        private const int TIPO_OPERARIO = 1;
        private const int TIPO_TECNICO = 2;
        private const int TIPO_PROFESIONAL = 3;

        // Estructura para almacenar estadísticas por tipo de empleado
        private class EstadisticasEmpleado
        {
            public int Cantidad { get; set; }
            public double AcumuladoSalarioNeto { get; set; }
        }

        static void Main(string[] args)
        {
            EjecutarAplicacion();
        }

        static void EjecutarAplicacion()
        {
            var estadisticas = InicializarEstadisticas();

            while (true)
            {
                int opcion = MostrarMenu();

                if (opcion == OPCION_SALIR)
                {
                    break;
                }
                else if (opcion == OPCION_AGREGAR_EMPLEADO)
                {
                    ProcesarNuevoEmpleado(estadisticas);
                }
            }

            MostrarResumenFinal(estadisticas);
        }

        static Dictionary<int, EstadisticasEmpleado> InicializarEstadisticas()
        {
            return new Dictionary<int, EstadisticasEmpleado>
            {
                [TIPO_OPERARIO] = new EstadisticasEmpleado(),
                [TIPO_TECNICO] = new EstadisticasEmpleado(),
                [TIPO_PROFESIONAL] = new EstadisticasEmpleado()
            };
        }

        static int MostrarMenu()
        {
            Console.WriteLine("\n=== SISTEMA DE GESTIÓN DE EMPLEADOS ===");
            Console.WriteLine("1. Agregar Empleado");
            Console.WriteLine("2. Salir");
            Console.Write("Seleccione una opción: ");

            if (int.TryParse(Console.ReadLine(), out int opcion) && (opcion == 1 || opcion == 2))
            {
                return opcion;
            }

            Console.WriteLine(" Opción no válida. Por favor ingrese 1 o 2.");
            return -1;
        }

        static void ProcesarNuevoEmpleado(Dictionary<int, EstadisticasEmpleado> estadisticas)
        {
            if (AgregarEmpleado(out int tipoEmpleado, out double salarioNeto))
            {
                ActualizarEstadisticas(estadisticas, tipoEmpleado, salarioNeto);
            }
        }

        static bool AgregarEmpleado(out int tipoEmpleado, out double salarioNeto)
        {
            tipoEmpleado = 0;
            salarioNeto = 0;

            Console.WriteLine("\n--- REGISTRO DE NUEVO EMPLEADO ---");

            string cedula = SolicitarCadena("Ingrese la cédula del empleado: ");
            string nombre = SolicitarCadena("Ingrese el nombre del empleado: ");

            if (!SolicitarTipoEmpleado(out tipoEmpleado) ||
                !SolicitarSalarioActual(out double salarioActual))
            {
                return false;
            }

            var calculo = CalcularSalarios(salarioActual, tipoEmpleado);
            salarioNeto = calculo.SalarioNeto;

            MostrarDetalleEmpleado(nombre, cedula, salarioActual, calculo);

            return true;
        }

        static string SolicitarCadena(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        static bool SolicitarTipoEmpleado(out int tipoEmpleado)
        {
            Console.WriteLine("\nTipos de empleado disponibles:");
            Console.WriteLine("1. Operario (15% de aumento)");
            Console.WriteLine("2. Técnico (10% de aumento)");
            Console.WriteLine("3. Profesional (5% de aumento)");
            Console.Write("Seleccione el tipo de empleado: ");

            if (int.TryParse(Console.ReadLine(), out tipoEmpleado) &&
                tipoEmpleado >= TIPO_OPERARIO && tipoEmpleado <= TIPO_PROFESIONAL)
            {
                return true;
            }

            Console.WriteLine(" Tipo de empleado no válido.");
            return false;
        }

        static bool SolicitarSalarioActual(out double salarioActual)
        {
            Console.Write("Ingrese el salario actual: ");

            if (double.TryParse(Console.ReadLine(), out salarioActual) && salarioActual >= 0)
            {
                return true;
            }

            Console.WriteLine(" Salario no válido. Debe ser un número positivo.");
            return false;
        }

        static (double Aumento, double SalarioBruto, double Deduccion, double SalarioNeto)
            CalcularSalarios(double salarioActual, int tipoEmpleado)
        {
            double porcentajeAumento = ObtenerPorcentajeAumento(tipoEmpleado);
            double aumento = salarioActual * porcentajeAumento;
            double salarioBruto = salarioActual + aumento;
            double deduccion = salarioBruto * (DEDUCCION_PORCENTAJE / 100);
            double salarioNeto = salarioBruto - deduccion;

            return (aumento, salarioBruto, deduccion, salarioNeto);
        }

        static double ObtenerPorcentajeAumento(int tipoEmpleado)
        {
            return tipoEmpleado switch
            {
                TIPO_OPERARIO => 0.15,
                TIPO_TECNICO => 0.10,
                TIPO_PROFESIONAL => 0.05,
                _ => 0
            };
        }

        static void MostrarDetalleEmpleado(string nombre, string cedula, double salarioActual,
                                          (double Aumento, double SalarioBruto, double Deduccion, double SalarioNeto) calculo)
        {
            Console.WriteLine("\n--- DETALLE DEL EMPLEADO ---");
            Console.WriteLine($"Empleado: {nombre}");
            Console.WriteLine($"Cédula: {cedula}");
            Console.WriteLine($"Salario Actual: {salarioActual:C}");
            Console.WriteLine($"Aumento: {calculo.Aumento:C}");
            Console.WriteLine($"Nuevo Salario Bruto: {calculo.SalarioBruto:C}");
            Console.WriteLine($"Deducciones ({DEDUCCION_PORCENTAJE}%): {calculo.Deduccion:C}");
            Console.WriteLine($"Salario Neto: {calculo.SalarioNeto:C}");
            Console.WriteLine("----------------------------");
        }

        static void ActualizarEstadisticas(Dictionary<int, EstadisticasEmpleado> estadisticas, int tipoEmpleado, double salarioNeto)
        {
            if (estadisticas.ContainsKey(tipoEmpleado))
            {
                estadisticas[tipoEmpleado].Cantidad++;
                estadisticas[tipoEmpleado].AcumuladoSalarioNeto += salarioNeto;
            }
        }

        static void MostrarResumenFinal(Dictionary<int, EstadisticasEmpleado> estadisticas)
        {
            Console.WriteLine("\n=== RESUMEN FINAL ===");

            MostrarResumenTipo("Operario", estadisticas[TIPO_OPERARIO]);
            MostrarResumenTipo("Técnico", estadisticas[TIPO_TECNICO]);
            MostrarResumenTipo("Profesional", estadisticas[TIPO_PROFESIONAL]);

            Console.WriteLine("¡Gracias por usar el sistema!");
        }

        static void MostrarResumenTipo(string tipo, EstadisticasEmpleado stats)
        {
            double promedio = stats.Cantidad > 0 ? stats.AcumuladoSalarioNeto / stats.Cantidad : 0;

            Console.WriteLine($"\n{tipo}:");
            Console.WriteLine($"   Cantidad Empleados: {stats.Cantidad}");
            Console.WriteLine($"   Acumulado Salario Neto: {stats.AcumuladoSalarioNeto:C}");
            Console.WriteLine($"   Promedio Salario Neto: {promedio:C}");
        }
    }
}