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


