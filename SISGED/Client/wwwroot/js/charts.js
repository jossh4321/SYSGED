function CreateColumnChart() {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdiv", am4charts.XYChart3D);

        // Add data
        chart.data = [{
            "country": "USA",
            "visits": 4025
        }, {
            "country": "China",
            "visits": 1882
        }, {
            "country": "Japan",
            "visits": 1809
        }, {
            "country": "Germany",
            "visits": 1322
        }, {
            "country": "UK",
            "visits": 1122
        }, {
            "country": "France",
            "visits": 1114
        }, {
            "country": "India",
            "visits": 984
        }, {
            "country": "Spain",
            "visits": 711
        }, {
            "country": "Netherlands",
            "visits": 665
        }, {
            "country": "Russia",
            "visits": 580
        }, {
            "country": "South Korea",
            "visits": 443
        }, {
            "country": "Canada",
            "visits": 441
        }, {
            "country": "Brazil",
            "visits": 395
        }, {
            "country": "Italy",
            "visits": 386
        }, {
            "country": "Australia",
            "visits": 384
        }, {
            "country": "Taiwan",
            "visits": 338
        }, {
            "country": "Poland",
            "visits": 328
        }];

        // Create axes
        let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "country";
        categoryAxis.renderer.labels.template.rotation = 270;
        categoryAxis.renderer.labels.template.hideOversized = false;
        categoryAxis.renderer.minGridDistance = 20;
        categoryAxis.renderer.labels.template.horizontalCenter = "right";
        categoryAxis.renderer.labels.template.verticalCenter = "middle";
        categoryAxis.tooltip.label.rotation = 270;
        categoryAxis.tooltip.label.horizontalCenter = "right";
        categoryAxis.tooltip.label.verticalCenter = "middle";

        let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.title.text = "Countries";
        valueAxis.title.fontWeight = "bold";

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.dataFields.valueY = "visits";
        series.dataFields.categoryX = "country";
        series.name = "Visits";
        series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;

        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;
        columnTemplate.stroke = am4core.color("#FFFFFF");

        columnTemplate.adapter.add("fill", function (fill, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        columnTemplate.adapter.add("stroke", function (stroke, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineY.strokeOpacity = 0;

    }); // end am4core.ready()
}

function fromCSHARPtoJS(num) {
    var n = "Valor desede JS: " + num;
    console.log(n);
    return n;
}

function DotNetInstanceTest(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("fromJStoCSHARP").then(res => {
        console.log(res);
    });
}


function GraficaColumnasDocumentos(documentos) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdiv", am4charts.XYChart3D);
        chart.labels.template.fill = am4core.color("white");
        // Add data
        chart.data = [{
            "country": "USA",
            "visits": 4025
        }, {
            "country": "China",
            "visits": 1882
        }, {
            "country": "Japan",
            "visits": 1809
        }, {
            "country": "Germany",
            "visits": 1322
        }, {
            "country": "UK",
            "visits": 1122
        }, {
            "country": "France",
            "visits": 1114
        }, {
            "country": "India",
            "visits": 984
        }, {
            "country": "Spain",
            "visits": 711
        }, {
            "country": "Netherlands",
            "visits": 665
        }, {
            "country": "Russia",
            "visits": 580
        }, {
            "country": "South Korea",
            "visits": 443
        }, {
            "country": "Canada",
            "visits": 441
        }, {
            "country": "Brazil",
            "visits": 395
        }, {
            "country": "Italy",
            "visits": 386
        }, {
            "country": "Australia",
            "visits": 384
        }, {
            "country": "Taiwan",
            "visits": 338
        }, {
            "country": "Poland",
            "visits": 328
        }];

        // Create axes
        let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "country";
        let labelTemplate = categoryAxis.renderer.labels.template;

        
        labelTemplate.rotation = 270;
        labelTemplate.hideOversized = false;
        categoryAxis.renderer.minGridDistance = 20;
        labelTemplate.horizontalCenter = "right";
        labelTemplate.verticalCenter = "middle";
        labelTemplate.fill = am4core.color("#fff");
        labelTemplate.background.fill = am4core.color("#000");

        /*categoryAxis.renderer.labels.template.rotation = 270;
        categoryAxis.renderer.labels.template.hideOversized = false;
        categoryAxis.renderer.minGridDistance = 20;
        categoryAxis.renderer.labels.template.horizontalCenter = "right";
        categoryAxis.renderer.labels.template.verticalCenter = "middle";
        categoryAxis.renderer.labels.template.fill = am4core.color("#808080");*/


        categoryAxis.tooltip.label.rotation = 270;
        categoryAxis.tooltip.label.horizontalCenter = "right";
        categoryAxis.tooltip.label.verticalCenter = "middle";


        let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.title.text = "Countries";
        valueAxis.title.fontWeight = "bold";

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.dataFields.valueY = "visits";
        series.dataFields.categoryX = "country";
        series.name = "Visits";
        series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;

        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;
        columnTemplate.stroke = am4core.color("#FFFFFF");

        columnTemplate.adapter.add("fill", function (fill, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        columnTemplate.adapter.add("stroke", function (stroke, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineY.strokeOpacity = 0;

    }); // end am4core.ready()
}
//Estadistica DOC X Mes
async function  ServiceCallBack_DocXMes(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getDocxXMes").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            GraficaDocXMes(res);
        }
    });
    return number
}

function GraficaDocXMes(estadisticas) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdivdocxmes", am4charts.XYChart3D);

        // Add data
        chart.data = estadisticas;

        // Create axes
        let documentAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        documentAxis.dataFields.category = "id";
        documentAxis.renderer.labels.template.rotation = 270;
        documentAxis.renderer.labels.template.hideOversized = false;
        documentAxis.renderer.minGridDistance = 20;
        documentAxis.renderer.labels.template.horizontalCenter = "right";
        documentAxis.renderer.labels.template.verticalCenter = "middle";
        documentAxis.tooltip.label.rotation = 270;
        documentAxis.tooltip.label.horizontalCenter = "right";
        documentAxis.tooltip.label.verticalCenter = "middle";

        let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.title.text = "Documentos Procesados";
        valueAxis.title.fontWeight = "bold";

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.dataFields.valueY = "cantidad";
        series.dataFields.categoryX = "id";
        series.name = "Cantidades Procesadas";
        series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;

        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;
        columnTemplate.stroke = am4core.color("#FFFFFF");

        columnTemplate.adapter.add("fill", function (fill, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        columnTemplate.adapter.add("stroke", function (stroke, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineY.strokeOpacity = 0;

    }); // end am4core.ready()
}
//Estadistica DOC X Mes X Area
async function ServiceCallBack_DocXMesXArea(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getDocxXMesXArea").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            GraficaDocXMesXArea(res);
        }
    });
    return number
}

function GraficaDocXMesXArea(estadisticas) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdivdocxmes", am4charts.XYChart3D);

        // Add data
        chart.data = estadisticas;

        // Create axes
        let documentAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        documentAxis.dataFields.category = "id";
        documentAxis.renderer.labels.template.rotation = 270;
        documentAxis.renderer.labels.template.hideOversized = false;
        documentAxis.renderer.minGridDistance = 20;
        documentAxis.renderer.labels.template.horizontalCenter = "right";
        documentAxis.renderer.labels.template.verticalCenter = "middle";
        documentAxis.tooltip.label.rotation = 270;
        documentAxis.tooltip.label.horizontalCenter = "right";
        documentAxis.tooltip.label.verticalCenter = "middle";

        let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.title.text = "Documentos Procesados";
        valueAxis.title.fontWeight = "bold";

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.dataFields.valueY = "cantidad";
        series.dataFields.categoryX = "id";
        series.name = "Cantidades Procesadas";
        series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;

        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;
        columnTemplate.stroke = am4core.color("#FFFFFF");

        columnTemplate.adapter.add("fill", function (fill, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        columnTemplate.adapter.add("stroke", function (stroke, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineY.strokeOpacity = 0;

    }); // end am4core.ready()
}
// Grafica de DOCS evaluados por Mes por junta directiva


async function ServiceCallBack_DocEvaluadoXMes(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getDocEvaluadosXMes").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            var lista = res;
            lista.forEach(x => generarGraficaCircular(x,x.id));
        }
    });
    return number
}
async function generarGraficaCircular(estadistica, etiqueta) {
    am4core.ready(function () {
        console.log(estadistica);
        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create(etiqueta, am4charts.PieChart);

        // Add data
        chart.data = [{
            "evaluacion": "Aprobados",
            "cantidad": estadistica.aprobados
        }, {
            "evaluacion": "Rechazados",
            "cantidad": estadistica.rechazados
        }];

        // Add and configure Series
        var pieSeries = chart.series.push(new am4charts.PieSeries());
        pieSeries.dataFields.value = "cantidad";
        pieSeries.dataFields.category = "evaluacion";
        pieSeries.slices.template.stroke = am4core.color("#fff");
        pieSeries.slices.template.strokeOpacity = 1;

        // This creates initial animation
        pieSeries.hiddenState.properties.opacity = 1;
        pieSeries.hiddenState.properties.endAngle = -90;
        pieSeries.hiddenState.properties.startAngle = -90;

        chart.hiddenState.properties.radius = am4core.percent(0);


    }); // end am4core.ready()
}


// Grafica de DOCS caducados x mes


async function ServiceCallBack_DocCaducadosXMes(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getDocCaducadosXMes").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            GraficaDocCaducadosXMes(res);
        }
    });
    return number
}
function GraficaDocCaducadosXMes(estadisticas) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdivdoccaducadoxmes", am4charts.XYChart3D);

        // Add data
        chart.data = estadisticas;

        // Create axes
        let documentAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        documentAxis.dataFields.category = "id";
        documentAxis.renderer.labels.template.rotation = 270;
        documentAxis.renderer.labels.template.hideOversized = false;
        documentAxis.renderer.minGridDistance = 20;
        documentAxis.renderer.labels.template.horizontalCenter = "right";
        documentAxis.renderer.labels.template.verticalCenter = "middle";
        documentAxis.tooltip.label.rotation = 270;
        documentAxis.tooltip.label.horizontalCenter = "right";
        documentAxis.tooltip.label.verticalCenter = "middle";

        let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.title.text = "Documentos Caducados";
        valueAxis.title.fontWeight = "bold";

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.dataFields.valueY = "caducados";
        series.dataFields.categoryX = "id";
        series.name = "Cantidades Caducadas";
        series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;

        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;
        columnTemplate.stroke = am4core.color("#FFFFFF");

        columnTemplate.adapter.add("fill", function (fill, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        columnTemplate.adapter.add("stroke", function (stroke, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineY.strokeOpacity = 0;

    }); // end am4core.ready()
}

// GANTT CHART SAMPLE
async function ServiceCallBack_expedientesgant(dotnetHelper) {
    var number;
    await ganttSample();
    /*await dotnetHelper.invokeMethodAsync("getDocCaducadosXMes").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            GraficaDocCaducadosXMes(res);
        }
    });
    return number*/
}

async function ganttSample() {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("ganttsample", am4charts.XYChart);
        chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

        chart.paddingRight = 30;
        chart.dateFormatter.inputDateFormat = "yyyy-MM-dd HH:mm";

        var colorSet = new am4core.ColorSet();
        colorSet.saturation = 0.4;

        chart.data = [{
            "category": "Module #1",
            "start": "2016-01-01",
            "end": "2016-01-14",
            "color": colorSet.getIndex(0).brighten(0),
            "task": "Gathering requirements"
        }, {
            "category": "Module #1",
            "start": "2016-01-16",
            "end": "2016-01-27",
            "color": colorSet.getIndex(0).brighten(0.4),
            "task": "Producing specifications"
        }, {
            "category": "Module #1",
            "start": "2016-02-05",
            "end": "2016-04-18",
            "color": colorSet.getIndex(0).brighten(0.8),
            "task": "Development"
        }, {
            "category": "Module #1",
            "start": "2016-04-18",
            "end": "2016-04-30",
            "color": colorSet.getIndex(0).brighten(1.2),
            "task": "Testing and QA"
        }, {
            "category": "Module #2",
            "start": "2016-01-08",
            "end": "2016-01-10",
            "color": colorSet.getIndex(2).brighten(0),
            "task": "Gathering requirements"
        }, {
            "category": "Module #2",
            "start": "2016-01-12",
            "end": "2016-01-15",
            "color": colorSet.getIndex(2).brighten(0.4),
            "task": "Producing specifications"
        }, {
            "category": "Module #2",
            "start": "2016-01-16",
            "end": "2016-02-05",
            "color": colorSet.getIndex(2).brighten(0.8),
            "task": "Development"
        }, {
            "category": "Module #2",
            "start": "2016-02-10",
            "end": "2016-02-18",
            "color": colorSet.getIndex(2).brighten(1.2),
            "task": "Testing and QA"
        }, {
            "category": "Module #3",
            "start": "2016-01-02",
            "end": "2016-01-08",
            "color": colorSet.getIndex(4).brighten(0),
            "task": "Gathering requirements"
        }, {
            "category": "Module #3",
            "start": "2016-01-08",
            "end": "2016-01-16",
            "color": colorSet.getIndex(4).brighten(0.4),
            "task": "Producing specifications"
        }, {
            "category": "Module #3",
            "start": "2016-01-19",
            "end": "2016-03-01",
            "color": colorSet.getIndex(4).brighten(0.8),
            "task": "Development"
        }, {
            "category": "Module #3",
            "start": "2016-03-12",
            "end": "2016-04-05",
            "color": colorSet.getIndex(4).brighten(1.2),
            "task": "Testing and QA"
        }, {
            "category": "Module #4",
            "start": "2016-01-01",
            "end": "2016-01-19",
            "color": colorSet.getIndex(6).brighten(0),
            "task": "Gathering requirements"
        }, {
            "category": "Module #4",
            "start": "2016-01-19",
            "end": "2016-02-03",
            "color": colorSet.getIndex(6).brighten(0.4),
            "task": "Producing specifications"
        }, {
            "category": "Module #4",
            "start": "2016-03-20",
            "end": "2016-04-25",
            "color": colorSet.getIndex(6).brighten(0.8),
            "task": "Development"
        }, {
            "category": "Module #4",
            "start": "2016-04-27",
            "end": "2016-05-15",
            "color": colorSet.getIndex(6).brighten(1.2),
            "task": "Testing and QA"
        }, {
            "category": "Module #5",
            "start": "2016-01-01",
            "end": "2016-01-12",
            "color": colorSet.getIndex(8).brighten(0),
            "task": "Gathering requirements"
        }, {
            "category": "Module #5",
            "start": "2016-01-12",
            "end": "2016-01-19",
            "color": colorSet.getIndex(8).brighten(0.4),
            "task": "Producing specifications"
        }, {
            "category": "Module #5",
            "start": "2016-01-19",
            "end": "2016-03-01",
            "color": colorSet.getIndex(8).brighten(0.8),
            "task": "Development"
        }, {
            "category": "Module #5",
            "start": "2016-03-08",
            "end": "2016-03-30",
            "color": colorSet.getIndex(8).brighten(1.2),
            "task": "Testing and QA"
        }];

        chart.dateFormatter.dateFormat = "yyyy-MM-dd";
        chart.dateFormatter.inputDateFormat = "yyyy-MM-dd";

        var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "category";
        categoryAxis.renderer.grid.template.location = 0;
        categoryAxis.renderer.inversed = true;

        var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
        dateAxis.renderer.minGridDistance = 70;
        dateAxis.baseInterval = { count: 1, timeUnit: "day" };
        // dateAxis.max = new Date(2018, 0, 1, 24, 0, 0, 0).getTime();
        //dateAxis.strictMinMax = true;
        dateAxis.renderer.tooltipLocation = 0;

        var series1 = chart.series.push(new am4charts.ColumnSeries());
        series1.columns.template.height = am4core.percent(70);
        series1.columns.template.tooltipText = "{task}: [bold]{openDateX}[/] - [bold]{dateX}[/]";

        series1.dataFields.openDateX = "start";
        series1.dataFields.dateX = "end";
        series1.dataFields.categoryY = "category";
        series1.columns.template.propertyFields.fill = "color"; // get color from data
        series1.columns.template.propertyFields.stroke = "color";
        series1.columns.template.strokeOpacity = 1;

        chart.scrollbarX = new am4core.Scrollbar();

    }); // end am4core.ready()
}