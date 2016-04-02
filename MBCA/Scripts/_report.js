var path = window.location.pathname;
//var path = "";
//var aseliTable = $("table").html(); 
//var tablee = tablee = $("table").DataTable({ "scrollX": true });
$("#DataTables_Table_0_wrapper").hide();

function loadTableData(data) {
    console.log(data);
    var buka = '<tr>';
    var tutup = '</tr>';
    $("table").html(null);
    tablee.destroy();

    $.post(path + '/Reporting', data)
        .done(function (res) {
            //$("table").html(aseliTable);
            var a = res;

            var countUnit = 0;

            for (var index in a.unit) {
                countUnit++;
                //$("table thead tr").append('<th>' + a.unit[index] + '</th>');
            }

            for (var index in a.data) {
                var isi = '<td>' + a.data[index].tgl + '</td>' +
                '<td>' + a.data[index].vessel + '</td>';

                for (var i = 0; i < countUnit; i++) {
                    isi += '<td>' + a.data[index].data[i] + '</td>';
                };

                $("table").append(buka + isi + tutup);
            }
        })
        .always(function () {
            //$("table").show();
            //$("#collapseOne").collapse('hide')
            ////tablee = $("table").DataTable({
            //    dom: "Brtip",
            //    buttons: [
            //    {
            //        extend: "excelHtml5",
            //        text: "Excel",
            //        filename: "Reporttt",
            //    }
            //    ]
            //})
        })
}

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

        $.ajax({
            "url": "api/report/" + tgl1 + "/" + tgl2 + "/" + tipe + "/" + ves,
            "success": function (json) {
                //console.log(json.data[3].datax[1]);
                var tableHeaders;
                $.each(json.columns, function (i, val) {
                    //console.log(val);
                    tableHeaders += "<th>" + val + "</th>";
                });

                $("#reportUnitTable").empty();
                $("#reportUnitTable").append('<table id="rUnit" class="table display table-bordered" style="width:100%" ><thead><tr>' + tableHeaders + '</tr></thead></table>');

                $.each(json.data, function(i,val){
                    //console.log(val.tg);
                    var isi = '<td>' + val.tg + '</td><td>' + val.ves + '</td>';
                    for (var k = 0; k < val.datax.length; k++) {
                        //console.log(val.datax[k]);
                        isi += '<td>' + val.datax[k] + '</td>';
                    }
                    $("#rUnit").append('<tr>' + isi + '</tr>');
                });

                var d = $('#rUnit').dataTable({
                    dom: "Brtip",
                    buttons: [
                        {
                            extend: "excelHtml5",
                            text: "Excel",
                            filename: "Reporttt",
                        }
                    ]
                });

                //d.column().data()
            },

            //"dataType": "json"
        });


        /*
        //console.log(tgl1, tgl2, tipe, ves);
        reportUnit.destroy();
        $.ajax({
            url: "api/report/" + tgl1 + "/" + tgl2 + "/" + tipe + "/" + ves,
            type: "post",
            //success: function (s) {
            //    alert("adsasdasd");
            //}
        })
        .done(function (dt) {
            //alert("Data Saved: ");
            //console.log(dt.unit);

            $.each(dt.unit, function (key, value) {
                //console.log(key + ": " + value);
                $("#reportUnitTable thead tr").append('<th>' + value + '</th>');
            });

            //$.each
            for (var data in dt.data) {
                console.log(dt.data[data].tg + "  === " + dt.data[data].ves);
                var isi = "<td>" + dt.data[data].tg + "</td>" + "<td>" + dt.data[data].ves + "</td>";
                var dtx = dt.data[data].datax;
                console.log(isi);
                $.each(dtx, function (k, v) {
                    console.log(k + ": " + v);
                    isi += "<td>" + v + "</td>";
                    //$("#reportUnitTable thead tr").append('<th>' + value + '</th>');
                });
                console.log(isi);
                $("#reportUnitTable ").append('<tr>' + isi + '</tr>');

            }
        })
        .always(function () {
            //$("table").show();
            //$("#collapseOne").collapse('hide')
            $("reportUnitTable").DataTable({
                dom: "Brtip",
                buttons: [
                {
                    extend: "excelHtml5",
                    text: "Excel",
                    filename: "Reporttt",
                }
                ]
            })
        });
        */
    });

})