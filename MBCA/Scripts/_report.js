var path = window.location.pathname;
//var path = "";
var aseliTable = $("table").html(); 
var tablee = tablee = $("table").DataTable({ "scrollX": true });
$("#DataTables_Table_0_wrapper").hide();

function loadTableData(data) {
    console.log(data);
    var buka = '<tr>';
    var tutup = '</tr>';
    $("table").html(null);
    tablee.destroy();

    $.post(path + '/Reporting', data)
        .done(function (res) {
            $("table").html(aseliTable);
            var a = res;

            var countUnit = 0;

            for (var index in a.unit) {
                countUnit++;
                $("table thead tr").append('<th>' + a.unit[index] + '</th>');
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
            $("table").show();
            $("#collapseOne").collapse('hide')
            tablee = $("table").DataTable({
                dom: "Brtip",
                buttons: [
                {
                    extend: "excelHtml5",
                    text: "Excel",
                    filename: "Reporttt",
                }
                ]
            })
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
        //var t
        var tgl1 = tg1.getFullYear() + "-" + (((tg1.getMonth() + 1) < 10) ? ("0" + (tg1.getMonth() + 1)) : (tg1.getMonth() + 1)) + "-" + ((tg1.getDate() < 10) ? ("0" + tg1.getDate()) : tg1.getDate());
        var tg2 = new Date($("#f_generateReport input[name='tgTo']").val());
        var tgl2 = tg2.getFullYear() + "-" + (((tg2.getMonth() + 1) < 10) ? ("0" + (tg2.getMonth() + 1)) : (tg2.getMonth() + 1)) + "-" + ((tg2.getDate() < 10) ? ("0" + tg2.getDate()) : tg2.getDate());

        console.log(tgl1, tgl2, tipe, ves);

        $.ajax({
            url: "api/report/"+tgl1+"/"+tgl2+"/"+tipe+"/"+ves,
            type : "post"
        })

        //reportUnit.ajax.url('api/report/tgl1/tgl2/tipe/ves');
            //unitTable.ajax.url('api/unit/' + hari).load();

        //var data = $("#f_generateReport").serialize();
        ////console.log(data);
        //$.post("/api/report", data, function () {
        //    //console.log("buaj jsonnn");
        //    //alert(res);
        //    //console.log(res);
        //    //$.get("/api/report", function (data) {
        //    //    console.log(data);
        //    //});
        //    alert("OK");
        //})
        //    //.done(
        //    //    alert("asdasdasd");
        //    //);
        //    .done(function () {
        //    //console.log(res);
        //    alert("aaaaa");

        //});
    });
    //$("#report1").click(function (e) {
    //    var data1 = $("#f_generateReport").serialize();
    //    //console.log(data);
    //    //$.post("/api/report", data, function (res) {
    //    //    console.log("buaj jsonnn");
    //    //    //alert(res);
    //    //    console.log(res);
    //    //    //$.get("/api/report", function (data) {
    //    //    //    console.log(data);
    //    //    //});
    //    //});
    //    $.ajax({
    //        type: "POST",
    //        contentType: "application/json; charset=utf-8",
    //        url: "report/_reportSatu",
    //        dataType: "json",
    //        data: data1,
    //        success: function (data) {
    //            console.log(data);
    //        }
    //    });
    //});
    var reportUnit = $("#reportUnitTable").DataTable({
        ajax : {
            url: "api/report/2016-03-01/2016-03-31/fl/"+0,
            type: "post"
        },
        select : true
    });

    //userTable = $("#userTable").DataTable({
    //    //dom: "<'top'Bf>rt<'bottom'lp><'clear'>",
    //    dom: "Brftip",
    //    ajax: {
    //        url: "api/users",
    //        type: 'post'
    //    },
    //    select: true,
})