//Estadistica 1
async function ServiceCallBack_DocEstadoXMes_EN(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getDocsEstadoxMes").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            console.log(res);
            //funcion();
           createChartDocsEstadoxMes(res);
        }
    });
    return number
}
function createChartDocsEstadoxMes(estadistica) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("chartdivdocestado", am4charts.XYChart);
        chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

        chart.data = estadistica;

        chart.colors.step = 2;
        chart.padding(30, 30, 10, 30);
        chart.legend = new am4charts.Legend();

        var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "id";
        categoryAxis.renderer.grid.template.location = 0;

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.min = 0;
        valueAxis.max = 100;
        valueAxis.strictMinMax = true;
        valueAxis.calculateTotals = true;
        valueAxis.renderer.minWidth = 50;


        var series1 = chart.series.push(new am4charts.ColumnSeries());
        series1.columns.template.width = am4core.percent(80);
        series1.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series1.name = "Documentos Atrazados";
        series1.dataFields.categoryX = "id";
        series1.dataFields.valueY = "caducados";
        series1.dataFields.valueYShow = "totalPercent";
        series1.dataItems.template.locations.categoryX = 0.5;
        series1.stacked = true;
        series1.tooltip.pointerOrientation = "vertical";

        var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
        bullet1.interactionsEnabled = false;
        bullet1.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet1.label.fill = am4core.color("#ffffff");
        bullet1.locationY = 0.5;

        var series2 = chart.series.push(new am4charts.ColumnSeries());
        series2.columns.template.width = am4core.percent(80);
        series2.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series2.name = "Documentos Pendientes";
        series2.dataFields.categoryX = "id";
        series2.dataFields.valueY = "pendientes";
        series2.dataFields.valueYShow = "totalPercent";
        series2.dataItems.template.locations.categoryX = 0.5;
        series2.stacked = true;
        series2.tooltip.pointerOrientation = "vertical";

        var bullet2 = series2.bullets.push(new am4charts.LabelBullet());
        bullet2.interactionsEnabled = false;
        bullet2.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet2.locationY = 0.5;
        bullet2.label.fill = am4core.color("#ffffff");

        var series3 = chart.series.push(new am4charts.ColumnSeries());
        series3.columns.template.width = am4core.percent(80);
        series3.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series3.name = "Documentos Procesados";
        series3.dataFields.categoryX = "id";
        series3.dataFields.valueY = "procesados";
        series3.dataFields.valueYShow = "totalPercent";
        series3.dataItems.template.locations.categoryX = 0.5;
        series3.stacked = true;
        series3.tooltip.pointerOrientation = "vertical";

        var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
        bullet3.interactionsEnabled = false;
        bullet3.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet3.locationY = 0.5;
        bullet3.label.fill = am4core.color("#ffffff");

        chart.scrollbarX = new am4core.Scrollbar();

    }); // end am4core.ready()
}


//Estadistica 2
async function ServiceCallBack_DocEstadoXMesXArea_EN(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getDocsEstadoxMesxArea").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            console.log(res);
            //funcion();
            createChartDocsEstadoxMesxArea(res);
        }
    });
    return number
}
function createChartDocsEstadoxMesxArea(estadistica) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("chartdivdocestadoxarea", am4charts.XYChart);
        chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

        chart.data = estadistica;

        chart.colors.step = 2;
        chart.padding(30, 30, 10, 30);
        chart.legend = new am4charts.Legend();

        var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "id";
        categoryAxis.renderer.grid.template.location = 0;

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.min = 0;
        valueAxis.max = 100;
        valueAxis.strictMinMax = true;
        valueAxis.calculateTotals = true;
        valueAxis.renderer.minWidth = 50;


        var series1 = chart.series.push(new am4charts.ColumnSeries());
        series1.columns.template.width = am4core.percent(80);
        series1.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series1.name = "Documentos Atrazados";
        series1.dataFields.categoryX = "id";
        series1.dataFields.valueY = "caducados";
        series1.dataFields.valueYShow = "totalPercent";
        series1.dataItems.template.locations.categoryX = 0.5;
        series1.stacked = true;
        series1.tooltip.pointerOrientation = "vertical";

        var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
        bullet1.interactionsEnabled = false;
        bullet1.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet1.label.fill = am4core.color("#ffffff");
        bullet1.locationY = 0.5;

        var series2 = chart.series.push(new am4charts.ColumnSeries());
        series2.columns.template.width = am4core.percent(80);
        series2.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series2.name = "Documentos Pendientes";
        series2.dataFields.categoryX = "id";
        series2.dataFields.valueY = "pendientes";
        series2.dataFields.valueYShow = "totalPercent";
        series2.dataItems.template.locations.categoryX = 0.5;
        series2.stacked = true;
        series2.tooltip.pointerOrientation = "vertical";

        var bullet2 = series2.bullets.push(new am4charts.LabelBullet());
        bullet2.interactionsEnabled = false;
        bullet2.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet2.locationY = 0.5;
        bullet2.label.fill = am4core.color("#ffffff");

        var series3 = chart.series.push(new am4charts.ColumnSeries());
        series3.columns.template.width = am4core.percent(80);
        series3.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series3.name = "Documentos Procesados";
        series3.dataFields.categoryX = "id";
        series3.dataFields.valueY = "procesados";
        series3.dataFields.valueYShow = "totalPercent";
        series3.dataItems.template.locations.categoryX = 0.5;
        series3.stacked = true;
        series3.tooltip.pointerOrientation = "vertical";

        var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
        bullet3.interactionsEnabled = false;
        bullet3.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet3.locationY = 0.5;
        bullet3.label.fill = am4core.color("#ffffff");

        chart.scrollbarX = new am4core.Scrollbar();

    }); // end am4core.ready()
}



//Estadistica 3
async function ServiceCallBack_DocEstadoXMesXUsuario_EN(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getDocsEstadoxMesxUsuario").then(res => {
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            console.log(res);
            //funcion();
            createChartDocsEstadoxMesxUsuario(res);
        }
    });
    return number
}
function createChartDocsEstadoxMesxUsuario(estadistica) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("chartdivdocestadoxusuario", am4charts.XYChart);
        chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

        chart.data = estadistica;

        chart.colors.step = 2;
        chart.padding(30, 30, 10, 30);
        chart.legend = new am4charts.Legend();

        var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "id";
        categoryAxis.renderer.grid.template.location = 0;

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        valueAxis.min = 0;
        valueAxis.max = 100;
        valueAxis.strictMinMax = true;
        valueAxis.calculateTotals = true;
        valueAxis.renderer.minWidth = 50;


        var series1 = chart.series.push(new am4charts.ColumnSeries());
        series1.columns.template.width = am4core.percent(80);
        series1.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series1.name = "Documentos Atrazados";
        series1.dataFields.categoryX = "id";
        series1.dataFields.valueY = "caducados";
        series1.dataFields.valueYShow = "totalPercent";
        series1.dataItems.template.locations.categoryX = 0.5;
        series1.stacked = true;
        series1.tooltip.pointerOrientation = "vertical";

        var bullet1 = series1.bullets.push(new am4charts.LabelBullet());
        bullet1.interactionsEnabled = false;
        bullet1.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet1.label.fill = am4core.color("#ffffff");
        bullet1.locationY = 0.5;

        var series2 = chart.series.push(new am4charts.ColumnSeries());
        series2.columns.template.width = am4core.percent(80);
        series2.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series2.name = "Documentos Pendientes";
        series2.dataFields.categoryX = "id";
        series2.dataFields.valueY = "pendientes";
        series2.dataFields.valueYShow = "totalPercent";
        series2.dataItems.template.locations.categoryX = 0.5;
        series2.stacked = true;
        series2.tooltip.pointerOrientation = "vertical";

        var bullet2 = series2.bullets.push(new am4charts.LabelBullet());
        bullet2.interactionsEnabled = false;
        bullet2.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet2.locationY = 0.5;
        bullet2.label.fill = am4core.color("#ffffff");

        var series3 = chart.series.push(new am4charts.ColumnSeries());
        series3.columns.template.width = am4core.percent(80);
        series3.columns.template.tooltipText =
            "{name}: {valueY.totalPercent.formatNumber('#.00')}%";
        series3.name = "Documentos Procesados";
        series3.dataFields.categoryX = "id";
        series3.dataFields.valueY = "procesados";
        series3.dataFields.valueYShow = "totalPercent";
        series3.dataItems.template.locations.categoryX = 0.5;
        series3.stacked = true;
        series3.tooltip.pointerOrientation = "vertical";

        var bullet3 = series3.bullets.push(new am4charts.LabelBullet());
        bullet3.interactionsEnabled = false;
        bullet3.label.text = "{valueY.totalPercent.formatNumber('#.00')}%";
        bullet3.locationY = 0.5;
        bullet3.label.fill = am4core.color("#ffffff");

        chart.scrollbarX = new am4core.Scrollbar();

    }); // end am4core.ready()
}

//gantt del cliente
function obtenercolor(color,tipodocumento) {
    var value = {};
    if (tipodocumento === "SolicitudBPN") {
        value = am4core.color(color).lighten(0.1);
    } else if (tipodocumento === "OficioBPN") {
        value = am4core.color(color).lighten(0.15);
    } else if (tipodocumento === "ResultadoBPN") {
        value = am4core.color(color).lighten(0.20);
    } else if (tipodocumento === "SolicitudExpedicionFirma") {
        value = am4core.color(color).lighten(0.25);
    } else if (tipodocumento === "OficioDesignacionNotario") {
        value = am4core.color(color).lighten(0.30);
    } else if (tipodocumento === "ConclusionFirma") {
        value = am4core.color(color).lighten(0.35);
    } else if (tipodocumento === "SolicitudDenuncia") {
        value = am4core.color(color).lighten(0.40);
    } else if (tipodocumento === "AperturamientoDisciplinario") {
        value = am4core.color(color).lighten(0.45);
    } else if (tipodocumento === "SolicitudExpedienteNotario") {
        value = am4core.color(color).lighten(0.50);
    } else if (tipodocumento === "EntregaExpedienteNotario") {
        value = am4core.color(color).lighten(0.05);
    } else if (tipodocumento === "Dictamen") {
        value = am4core.color(color).lighten(-0.05);
    } else if (tipodocumento === "Resolucion") {
        value = am4core.color(color).lighten(-0.1);
    } else if (tipodocumento === "Apelacion") {
        value = am4core.color(color).lighten(-0.15);
    } else {
        value = am4core.color(color).lighten(0.0);
    }
    return value;
}
async function ServiceCallBack_DocExpedienteProceso_EN(dotnetHelper) {
    var number;
    await dotnetHelper.invokeMethodAsync("getGanttCliente").then(res => {
        //console.log(res);
        var res2 = res.forEach(x => {
            x.fechacreacion = x.fechacreacion.split("T")[0];
            x.fechademora = x.fechademora.split("T")[0];
            x.fechaexceso = x.fechaexceso.split("T")[0];
            x.identificador = x.tipoexpediente + " - " + x.id;
            //x.color = "blue";
            if (x.estado === "pendiente") {
                /*if (x.tipodocumento === "SolicitudBPN") {
                    x.color = am4core.color("blue").lighten(0.1);
                } else if (x.tipodocumento === "OficioBPN") {
                    x.color = am4core.color("blue").lighten(0.15);
                } else if (x.tipodocumento === "ResultadoBPN") {
                    x.color = am4core.color("blue").lighten(0.20);
                } else if (x.tipodocumento === "SolicitudExpedicionFirma") {
                    x.color = am4core.color("blue").lighten(0.25);
                } else if (x.tipodocumento === "OficioDesignacionNotario") {
                    x.color = am4core.color("blue").lighten(0.30);
                } else if (x.tipodocumento === "ConclusionFirma") {
                    x.color = am4core.color("blue").lighten(0.35);
                } else if (x.tipodocumento === "SolicitudDenuncia") {
                    x.color = am4core.color("blue").lighten(0.40);
                } else if (x.tipodocumento === "AperturamientoDisciplinario") {
                    x.color = am4core.color("blue").lighten(0.45);
                } else if (x.tipodocumento === "SolicitudExpedienteNotario") {
                    x.color = am4core.color("blue").lighten(0.50);
                } else if (x.tipodocumento === "EntregaExpedienteNotario") {
                    x.color = am4core.color("blue").lighten(0.05);
                } else if (x.tipodocumento === "Dictamen") {
                    x.color = am4core.color("blue").lighten(-0.05);
                } else if (x.tipodocumento === "Resolucion") {
                    x.color = am4core.color("blue").lighten(-0.1);
                } else if (x.tipodocumento === "Apelacion") {
                    x.color = am4core.color("blue").lighten(-0.15);
                } else {
                    x.color = am4core.color("blue").lighten(0.0);
                }*/
                x.color = obtenercolor("blue", x.tipodocumento);
            }
            else if (x.estado === "procesado") { x.color = obtenercolor("green", x.tipodocumento);}
            else { x.color = obtenercolor("red", x.tipodocumento);}
        });
        console.log(res);
        if (res.length === 0) {
            number = 0;
        } else {
            number = 1;
            generarGantt(res);
        }
    });
    return number
}

function generarGantt(estadistica) {
    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("divgantt", am4charts.XYChart);
        chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

        chart.paddingRight = 30;
        chart.dateFormatter.inputDateFormat = "yyyy-MM-dd HH:mm";

        var colorSet = new am4core.ColorSet();
        colorSet.saturation = 0.4;

        chart.data = estadistica;
        /*chart.data = [{
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
        }];*/

        chart.dateFormatter.dateFormat = "yyyy-MM-dd";
        chart.dateFormatter.inputDateFormat = "yyyy-MM-dd";

        var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "identificador";
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
        series1.columns.template.tooltipText = "{tipodocumento}: [bold]{openDateX}[/] - [bold]{dateX}[/]";

        series1.dataFields.openDateX = "fechacreacion";
        series1.dataFields.dateX = "fechademora";
        series1.dataFields.categoryY = "identificador";
        series1.columns.template.propertyFields.fill = "color"; // get color from data
        series1.columns.template.propertyFields.stroke = "color";
        series1.columns.template.strokeOpacity = 1;

        chart.scrollbarX = new am4core.Scrollbar();

    }); // end am4core.ready()
}


