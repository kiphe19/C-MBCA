var path = window.location.pathname;
//var path = "";
//var aseliTable = $("table").html(); 
//var tablee = tablee = $("table").DataTable({ "scrollX": true });
//$("#DataTables_Table_0_wrapper").hide();

$(document).ready(function () {
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

        $.ajax({
            "url": "api/reportDC/" + tgl1 + "/" + tgl2,
            "success": function (json) {
                console.log(json);

                $('#tbDC').DataTable({
                    dom: "Brtip",
                    destroy: true,
                    data: json.data,
                    columns: [
                        {   
                            data : "unit",
                            title: "Unit"
                        },
                        {   
                            data : "litre" ,
                            title: "Fuel (L)",
                            render: function (d) {
                                return parseFloat(d).toFixed(3);
                            }
                        },
                        {   
                            data : "fuel",
                            title: "Fuel Price"
                        },
                        {   
                            data : "charter",
                            title: "Charter Price" 
                        },
                        { 
                            data : "boat",
                            title: "Boat Hours"
                        }
                    ],
                    buttons: [
                        {
                            extend: "excelHtml5",
                            text: "Excel D&C",
                            filename: "ReportDC_" + tgl1 + "_" + tgl2
                        }
                    ]
                });
            }
        });

    });
})