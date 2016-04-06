var path = window.location.pathname;
//var path = "";
//var aseliTable = $("table").html(); 
//var tablee = tablee = $("table").DataTable({ "scrollX": true });
//$("#DataTables_Table_0_wrapper").hide();

function loadTableMainUnit(tg1, tg2) {
    $.ajax({
        "url": "api/rMain/" + tg1 + "/" + tg2,
        "success": function (json) {
            //console.log(json);

            $('#tbMainUnit').DataTable({
                //dom: "Brtip",
                destroy: true,
                data: json.data,
                columns: [{
                        data: "main",
                        title: "Main Unit"
                    },{
                        data: "litre",
                        title: "Fuel (L)",
                        render: function (d) {
                            return parseFloat(d).toFixed(3);
                        }
                    },{
                        data: "fuel",
                        title: "Fuel Price"
                    },{
                        data: "charter",
                        title: "Charter Price"
                    }]
            });
        }
    });
}

function loadTableDrillComptUnit(tg1, tg2) {
    $.ajax({
        "url": "api/reportDC/" + tg1 + "/" + tg2,
        "success": function (json) {
            console.log(json);

            $('#tbDC').DataTable({
                //dom: "Brtip",
                destroy: true,
                data: json.data,
                columns: [{   
                        data : "unit",
                        title: "Unit"
                    },{   
                        data : "litre" ,
                        title: "Fuel (L)",
                        render: function (d) {
                            return parseFloat(d).toFixed(3);
                        }
                    },{   
                        data : "fuel",
                        title: "Fuel Price"
                    },{   
                        data : "charter",
                        title: "Charter Price" 
                    },{ 
                        data : "boat",
                        title: "Boat Hours"
                    }],
                //buttons: [
                //    {
                //        extend: "excelHtml5",
                //        text: "Excel D&C",
                //        filename: "ReportDC_" + tg1 + "_" + tg2
                //    }
                //]
            });
        }
    });
}

function loadTableDailyDrillUnit(tg1, tg2, unit) {
    $.ajax({
        "url": "api/reportDCU/" + tg1 + "/" + tg2+"/"+unit,
        "success": function (json) {
            console.log(json);

            $('#tbDCU').DataTable({
                dom: "Brtip",
                destroy: true,
                data: json.data,
                columns: [{
                    data: null,
                    title: "No"
                }, {
                    data: "tg",
                    title: "Date",
                    render: function (d) {
                        var date = new Date(d);
                        var month = date.getMonth() + 1;
                        var day = date.getDate();
                        return ((month < 10) ? (month + "0") : month) + "/" + ((day < 10)? ("0"+day) : day)  + "/" + date.getFullYear();
                    }
                },{
                    data: "unit",
                    title: "Unit"
                },{
                    data : "well",
                    title : "Well"
                },{
                    data : "afe",
                    title : "AFE"
                },{
                    data : "psc",
                    title : "PSC"
                },{
                    data : "start",
                    title: "From",
                    render: function (d) {
                        var date = new Date(d);
                        return ((date.getHours() < 10) ? ("0" + date.getHours()) : date.getHours()) + ":" + ((date.getMinutes() < 10) ? ("0" + date.getMinutes()) : date.getMinutes());
                    }
                },{
                    data : "end",
                    title: "To",
                    render: function (d) {
                        var date = new Date(d);
                        return ((date.getHours() < 10) ? ("0" + date.getHours()) : date.getHours()) + ":" + ((date.getMinutes() < 10) ? ("0" + date.getMinutes()) : date.getMinutes());
                    }
                },{
                    data: "litre",
                    title: "Fuel (L)",
                    render: function (d) {
                        return parseFloat(d).toFixed(3);
                    }
                }, {
                    data: "fuel",
                    title: "Fuel Price"
                }, {
                    data: "charter",
                    title: "Charter Price"
                }, {
                    data: "mob",
                    title: "Mob/Demob"
                }],
                buttons: [
                    {
                        extend: "excelHtml5",
                        text: "Excel D&C",
                        filename: "ReportDCU_" + tg1 + "_" + tg2
                    }
                ],
                fnDrawCallback: function (oSettings) {
                    if (oSettings.bSorted || oSettings.bFiltered) {
                        for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                            $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                        }
                    }
                }
            });
        }
    });
}

$(document).ready(function () {
    var tgl = new Date();
    var yyyy = tgl.getFullYear();
    var mm = (tgl.getMonth() + 1);
    var dd = (tgl.getDate() < 10) ? "0" + tgl.getDate() : tgl.getDate();
    var t1, t2;
    if (dd <= 25) {
        t1 = yyyy + "-" + (((mm - 1) < 10) ? "0" + (mm - 1) : (mm - 1)) + "-" + 25;
        t2 = yyyy + "-" + ((mm < 10) ? "0" + mm : mm) + "-" + 25;
    }
    else {
        t1 = yyyy + "-" + ((mm < 10) ? "0" + mm : mm) + "-" + 25;
        t2 = yyyy + "-" + (((mm + 1) < 10) ? "0" + (mm + 1) : (mm + 1)) + "-" + 25;
    }

    loadTableMainUnit(t1, t2);
    loadTableDrillComptUnit(t1, t2);


    //console.log("dibuka saat klik report");

    $("#formGenerate").submit(function (e) {
        var data = $(this).serialize();
        loadTableData(data);
        e.preventDefault();
    });

    $("#formGenerate input[type='text']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#accordion").on('hide.bs.collapse', function () {
        $("#accordion h4 i").removeClass("glyphicon-chevron-down");
        $("#accordion h4 i").addClass("glyphicon-chevron-up");
    })
    $("#accordion").on('show.bs.collapse', function () {
        $("#accordion h4 i").removeClass("glyphicon-chevron-up");
        $("#accordion h4 i").addClass("glyphicon-chevron-down");
    });
    $("#f_generateReport input[type='text']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#f_MainReport input[type='text']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#f_dcReport input[type='text']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    $("#f_RepDailyDC input[type='text']").datetimepicker({
        format: "MM/DD/YYYY"
    });
    

    $("#report1").click(function (e) {
        var ves = $("#f_generateReport select[name='vesselId'] option:selected").val();
        var tipe = $("#f_generateReport select[name='type'] option:selected").val();
        var tg1 = new Date($("#f_generateReport input[name='tgFrom']").val());
        var tgl1 = tg1.getFullYear() + "-" + (((tg1.getMonth() + 1) < 10) ? ("0" + (tg1.getMonth() + 1)) : (tg1.getMonth() + 1)) + "-" + ((tg1.getDate() < 10) ? ("0" + tg1.getDate()) : tg1.getDate());
        var tg2 = new Date($("#f_generateReport input[name='tgTo']").val());
        var tgl2 = tg2.getFullYear() + "-" + (((tg2.getMonth() + 1) < 10) ? ("0" + (tg2.getMonth() + 1)) : (tg2.getMonth() + 1)) + "-" + ((tg2.getDate() < 10) ? ("0" + tg2.getDate()) : tg2.getDate());
        //$('#tbUnit').DataTable().destroy();
        $.ajax({
            "url": "api/report/" + tgl1 + "/" + tgl2 + "/" + tipe + "/" + ves,
            "success": function (json) {
                var kolom = [];
                $.each(json.columns, function(i, value){
                    //var obj = { sTitle: value };
                    var obj = { title: value };
                    kolom.push(obj);
                });

                var dd = [];
                var aa = [];
                $.each(json.data, function (i, val) {
                    aa.push(val.tg);
                    aa.push(val.ves);
                    //aa.push(val.datax)
                    //console.log(val.datax);
                    for (var k = 0; k < val.datax.length; k++) {
                        //console.log(val.datax[k]);
                        aa.push(val.datax[k]);
                    }
                    dd.push(aa);
                    aa = [];
                });

                //console.log(dd);
                $('#tbUnit').DataTable({
                    dom: "Brtip",
                    destroy: true,
                    data: dd,
                    columns: kolom,
                    buttons: [
                        {
                            extend: "excelHtml5",
                            text: "Excel",
                            filename: "Report_" + tgl1 + "_" + tgl2
                        }
                    ]
                });
            },
            "dataType": "json"
        });
       

    });

    $("#reportMain").click(function () {
        //console.log("klik main unti report");
        
        var tg1 = new Date($("#f_MainReport input[name='main_tgFrom']").val());
        var tgl1 = tg1.getFullYear() + "-" + (((tg1.getMonth() + 1) < 10) ? ("0" + (tg1.getMonth() + 1)) : (tg1.getMonth() + 1)) + "-" + ((tg1.getDate() < 10) ? ("0" + tg1.getDate()) : tg1.getDate());
        var tg2 = new Date($("#f_MainReport input[name='main_tgTo']").val());
        var tgl2 = tg2.getFullYear() + "-" + (((tg2.getMonth() + 1) < 10) ? ("0" + (tg2.getMonth() + 1)) : (tg2.getMonth() + 1)) + "-" + ((tg2.getDate() < 10) ? ("0" + tg2.getDate()) : tg2.getDate());

        console.log("asd " + tgl1 + "  === " + tgl2);

        loadTableMainUnit(tgl1, tgl2);
    });

    $("#reportDC").click(function () {
        var tg1 = new Date($("#f_dcReport input[name='dc_tgFrom']").val());
        var tgl1 = tg1.getFullYear() + "-" + (((tg1.getMonth() + 1) < 10) ? ("0" + (tg1.getMonth() + 1)) : (tg1.getMonth() + 1)) + "-" + ((tg1.getDate() < 10) ? ("0" + tg1.getDate()) : tg1.getDate());
        var tg2 = new Date($("#f_dcReport input[name='dc_tgTo']").val());
        var tgl2 = tg2.getFullYear() + "-" + (((tg2.getMonth() + 1) < 10) ? ("0" + (tg2.getMonth() + 1)) : (tg2.getMonth() + 1)) + "-" + ((tg2.getDate() < 10) ? ("0" + tg2.getDate()) : tg2.getDate());

        //console.log("asd " + tgl1 + "  === " + tgl2);
        loadTableDrillComptUnit(tgl1, tgl2)
    });

    $("#report2").click(function () {
        var tg1 = new Date($("#f_RepDailyDC input[name='tgFrom']").val());
        var tgl1 = tg1.getFullYear() + "-" + (((tg1.getMonth() + 1) < 10) ? ("0" + (tg1.getMonth() + 1)) : (tg1.getMonth() + 1)) + "-" + ((tg1.getDate() < 10) ? ("0" + tg1.getDate()) : tg1.getDate());
        var tg2 = new Date($("#f_RepDailyDC input[name='tgTo']").val());
        var tgl2 = tg2.getFullYear() + "-" + (((tg2.getMonth() + 1) < 10) ? ("0" + (tg2.getMonth() + 1)) : (tg2.getMonth() + 1)) + "-" + ((tg2.getDate() < 10) ? ("0" + tg2.getDate()) : tg2.getDate());
        var unit = $("#f_RepDailyDC select[name='unitId'] option:selected").val();
        //var tipe = $("#f_generateReport select[name='type'] option:selected").val();
        console.log("==> " + tgl1 + " ===> " + tgl2 + " ====> " + unit);
        loadTableDailyDrillUnit(tgl1, tgl2,unit);

    });


})