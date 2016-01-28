var path = window.location.pathname;
var aseliTable = $("table").html();
var tablee = tablee = $("table").DataTable();
$("#DataTables_Table_0_wrapper").hide();

function loadTableData(data) {
    var buka = '<tr>';
    var tutup = '</tr>';
    $("table").html(null);
    tablee.destroy();

    $.get(path + '/json', { type: data })
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
    //loadTableData($("#ReportType").val());
    $("#vessel").change(function () {
        tablee.search($(this).val()).draw();
    })
    $("#ReportType").change(function () {
        loadTableData($(this).val());
    })
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
    })
})