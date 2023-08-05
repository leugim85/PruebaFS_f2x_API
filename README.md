# F2xFullstackAPI

Este proyecto ha sido desarrollado en .Net 7, utilizando el patron `Repository` y `UnitOfWork`, cuenta con un total de 4 capas, que se han distribuidas, asi: 
Dominio, Core,Infraestructura, API y un proyecto adicional en el que se ha implementado un Timer Worker que se encarga de la consulta automatica del EndPoint proporcionado para la prueba. 

## TimerWorker 

Permite la descarga de la información diariamente. Debido al rango en que se solicito la extracción de la data (mas de 700 dias), se vio la necesidad de implementar tecnícas para evitar errores por concurrencia del endpoint proporcionado, la ejecución del proceso en batch esta configurada para que mientras la aplicación este corriendo, se ejecute a las 6 am todos los dias para extraer la información y persistirla en base de datos, guardando la fecha de la ultima consulta ejecutada asi no se obtengan resultados para tal dia, asi si la aplicación presentara una falta de disponibilidad en ciertos dias, al recuperar su funcionamiento podra recuperar la información exactamente en el punto en que se ejecuto la ultima consulta, evitando asi perdida o inconsistencia de información.

## Persistencia de la información

Se ha implementado un esquema de 4 tablas en la base de datos, la primera es el historial de migraciones, la segunda es el registro de la ultima fecha de consulta y las dos restantes persisten la información descargada de los enpoint proporcionados.


## API

Se ha expuesto un endpoint que permite la consulta de el reporte, para el entorno local: `https://localhost:44372/api/VehicleCounter/GetVehicleCounterInformation`.
Este servicio puede recibir o no, un string para consultar por especificas estaciones, retornando un objeto de tipo:

```csharp
public class GeneralSummaryDto
    {
        public List<StationSummaryDto> VehicleCounterSummaryList { get; set; };

        public int TotalCarsGeneral { get; set; }

        public double TotalAmountGeneral { get; set; }

    }
    public class StationSummaryDto
    {
        public string Station { get; set; }
        public int TotalAmount { get; set; }
        public int VehicleCount { get; set;}

        public List<SummaryByDateDto> SummaryByDates { get; set; };
    }

    public class SummaryByDateDto
    {
        public int TotalAmountByDate { get; set; }

        public int VehicleCountByDate { get; set; }

        public DateOnly Date { get; set; }
    }
```
## Comentarios adicionales

Para el desarrollo de la solución se implementaron diferentes paquetes de Nuget para potenciar la capacidad del API, entre ellos, AutoFact, para simplificar 
la inyeccion de dependencias, AutoMapper para el mapeo de entidades y Dtos, ApplicationInsights para monitorear el comportamiento de la solución, especialmente para identificar los errores que se presentaron en el consumo de los servicios, lo cual permitio detectar los problemas de concurrencia e implementar medidas correctivas. 






