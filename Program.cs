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

        // Tipos de empleado (índices del array)
        private const int TIPO_OPERARIO = 0;
        private const int TIPO_TECNICO = 1;
        private const int TIPO_PROFESIONAL = 2;
        private const int TOTAL_TIPOS_EMPLEADO = 3;

        // Estructura para almacenar estadísticas por tipo de empleado
        private class EstadisticasEmpleado
        {
            public int Cantidad { get; set; }
            public double AcumuladoSalarioNeto { get; set; }
        }

        static void Main(string[] args) // Punto de entrada del programa
        {
            EjecutarAplicacion();
        }

        static void EjecutarAplicacion() // Método principal que ejecuta la lógica del programa, parte del método Main
        {
            
            EstadisticasEmpleado[] estadisticas = InicializarEstadisticasArray();

            while (true)
            {
                int opcion = MostrarMenu();

                if (opcion == OPCION_SALIR)
                {
                    break;
                }
                else if (opcion == OPCION_AGREGAR_EMPLEADO)
                {
                    ProcesarNuevoEmpleadoArray(estadisticas); // Procesar la adición de un nuevo empleado
                }
            }

            MostrarResumenFinalArray(estadisticas); // Mostrar el resumen final al salir
        }

       
        static EstadisticasEmpleado[] InicializarEstadisticasArray()
        {
            // Crear array de 3 elementos (uno por cada tipo de empleado)
            EstadisticasEmpleado[] arrayEstadisticas = new EstadisticasEmpleado[TOTAL_TIPOS_EMPLEADO];

            // Inicializar cada posición del array
            for (int i = 0; i < TOTAL_TIPOS_EMPLEADO; i++)
            {
                arrayEstadisticas[i] = new EstadisticasEmpleado();
            }

            return arrayEstadisticas;
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

        static void ProcesarNuevoEmpleadoArray(EstadisticasEmpleado[] estadisticas)
        {
            if (AgregarEmpleadoArray(out int tipoEmpleado, out double salarioNeto))
            {
                ActualizarEstadisticasArray(estadisticas, tipoEmpleado, salarioNeto);
            }
        }

        static bool AgregarEmpleadoArray(out int tipoEmpleado, out double salarioNeto)
        {
            tipoEmpleado = 0;
            salarioNeto = 0;

            Console.WriteLine("\n--- REGISTRO DE NUEVO EMPLEADO ---");

            string cedula = SolicitarCadena("Ingrese la cédula del empleado: ");
            string nombre = SolicitarCadena("Ingrese el nombre del empleado: ");

            if (!SolicitarTipoEmpleadoArray(out tipoEmpleado) ||
                !SolicitarSalarioActual(out double salarioActual))
            {
                return false;
            }

            var calculo = CalcularSalariosArray(salarioActual, tipoEmpleado);
            salarioNeto = calculo.SalarioNeto;

            MostrarDetalleEmpleado(nombre, cedula, salarioActual, calculo);

            return true;
        }

        static string SolicitarCadena(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }

        
        static bool SolicitarTipoEmpleadoArray(out int tipoEmpleado)
        {
            Console.WriteLine("\nTipos de empleado disponibles:");
            Console.WriteLine("1. Operario (15% de aumento)");
            Console.WriteLine("2. Técnico (10% de aumento)");
            Console.WriteLine("3. Profesional (5% de aumento)");
            Console.Write("Seleccione el tipo de empleado: ");

            if (int.TryParse(Console.ReadLine(), out int opcion) &&
                opcion >= 1 && opcion <= 3)
            {
                // Convertir opción del usuario (1,2,3) a índice del array (0,1,2)
                tipoEmpleado = opcion - 1;
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
            CalcularSalariosArray(double salarioActual, int tipoEmpleado)
        {
            // Array con los porcentajes de aumento para cada tipo
            double[] porcentajesAumento = { 0.15, 0.10, 0.05 }; // Operario, Técnico, Profesional

            double porcentajeAumento = porcentajesAumento[tipoEmpleado];
            double aumento = salarioActual * porcentajeAumento;
            double salarioBruto = salarioActual + aumento;
            double deduccion = salarioBruto * (DEDUCCION_PORCENTAJE / 100);
            double salarioNeto = salarioBruto - deduccion;

            return (aumento, salarioBruto, deduccion, salarioNeto);
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

        
        static void ActualizarEstadisticasArray(EstadisticasEmpleado[] estadisticas, int tipoEmpleado, double salarioNeto)
        {
            // Validar que el índice esté dentro del rango del array
            if (tipoEmpleado >= 0 && tipoEmpleado < estadisticas.Length)
            {
                estadisticas[tipoEmpleado].Cantidad++;
                estadisticas[tipoEmpleado].AcumuladoSalarioNeto += salarioNeto;
            }
        }

        
        static void MostrarResumenFinalArray(EstadisticasEmpleado[] estadisticas)
        {
            Console.WriteLine("\n=== RESUMEN FINAL ===");

            // Array con los nombres de los tipos de empleado
            string[] nombresTipos = { "Operario", "Técnico", "Profesional" };

            // Recorrer el array usando for
            for (int i = 0; i < estadisticas.Length; i++)
            {
                MostrarResumenTipoArray(nombresTipos[i], estadisticas[i]);
            }

            Console.WriteLine("¡Gracias por usar el sistema!");
        }

        static void MostrarResumenTipoArray(string tipo, EstadisticasEmpleado stats)
        {
            double promedio = stats.Cantidad > 0 ? stats.AcumuladoSalarioNeto / stats.Cantidad : 0;

            Console.WriteLine($"\n{tipo}:");
            Console.WriteLine($"   Cantidad Empleados: {stats.Cantidad}");
            Console.WriteLine($"   Acumulado Salario Neto: {stats.AcumuladoSalarioNeto:C}");
            Console.WriteLine($"   Promedio Salario Neto: {promedio:C}");
        }
    }
}