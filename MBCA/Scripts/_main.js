var editor,
    dailyTable,
    path = window.location.pathname,
    monthlyTable;

(function () {

    dailyEditor = new $.fn.dataTable.Editor({
        ajax: path + "/api/daily",
        table: "#dailyTable",
        fields: [
            { label: "Date", name: "daily_date", type: "datetime", format: "MM/DD/YYYY" },
            { label: "Vessel", name: "daily_vessel", type: "select" },
            { label: "Activity", name: "daily_activity", type: "select" },
            { label: "User Unit", name: "daily_unit", type: "select" },
            { label: "Duration", name: "daily_duration" }
        ]
    }).on('preOpen', function () {
        dailyEditor.title('Add new Daily Activity');
    })

    dailyTable = $("#dailyTable").DataTable({
        dom: '<"dailyButton"B<"floatright">>rtip',
        ajax: {
            url: path + "/api/daily",
            method: "post"
        },
        serverSide: true,
        columns: [
            { data: 'daily_date' },
            { data: 'daily_unit' },
            { data: "daily_activity" },
            { data: "daily_duration" }
        ],
        select: true,
        buttons: [
            {
                text: "Edit",
                action: function (e, dt, node, config) {
                    var a = dailyTable.rows('.selected').indexes()
                    if (a.length !== 0) {
                        var b = dailyTable.row(a).data();
                        $('#inputDaily').collapse('show');
                        $("#daily_unit").val(b.daily_unit);
                        $("#daily_activity").val(b.daily_activity);
                        $("#dailyForm input[name='daily_date']").val(b.daily_date);
                        $("#dailyForm input[name='daily_duration']").val(b.daily_duration);
                        $("#dailyForm input[name='action']").val("update");
                        $("#dailyForm input[name='id']").val(b.id);
                        $("#dailyForm input[name='daily_fuel']").val(b.daily_fuel);
                        $("#btnEditGroup").show();
                        $("#btnSaveGroup").hide();
                    }
                }
            },
            { extend: "remove", editor: dailyEditor },
        ]
    });


    monthlyEditor = new $.fn.dataTable.Editor({
        ajax: path + "/api/monthly",
        table: "#monthlyTable",
        fields: [
            { label: "Date", name: "monthly_date", type: "datetime", format: "DD/MM/YYYY" },
            { label: "Vessel", name: "monthly_vessel", type: "select" },
            { label: "Activity", name: "monthly_activity", type: "select" },
            { label: "User Unit", name: "monthly_unit", type: "select" },
            { label: "Duration", name: "monthly_duration" }
        ]
    })

    monthlyTable = $("#monthlyTable").DataTable({
        dom: 'B<"floatright">rtip',
        ajax: {
            url: path + "/api/monthly",
            type: "post"
        },
        columns: [
            { data: "monthly_date" },
            { data: "monthly_vessel" },
            { data: "monthly_activity" },
            { data: "monthly_unit" },
            { data: "monthly_duration" }
        ],
        select: true,
        buttons: [
            {
                extend: "collection",
                text: "Export to ..",
                buttons: ['excel']
            }
        ]
    })
})(jQuery)

$(document).ready(function () {

    $("#dailyForm").submit(function (e) {
        var data = $(this).serialize();
            $.post(path + "/api/cs/daily", data, function (res) {
                if (res === "success") {
                    dailyTable.ajax.reload();
                    $("#dailyForm input[name='daily_duration']").val(null);
                    dailyCancel.apply();
                } else {
                    alert(res);
                }
            })
        e.preventDefault();
    })

    $("#daily_date").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date()
    })
    $("#monthlyPanel input").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date()
    })
    $("#btnSaveDataDaily").click(function () {
        var data = dailyTable.data();

        var saveDaily = function () {
            $.post(path + "/api/save/daily")
            .success(function (res) {
                if (res) {
                    monthlyTable.ajax.reload();
                    dailyTable.ajax.reload();
                }
            })
        }

        if (data.length == 0) {
            alert("apa yang mau disimpen?");
        } else {
            var a = confirm("Are you sure, You want to save current data?");
            if (a) {
                var duration = 0,
                    downTime = false;

                for (var i = 0; i < data.length; i++) {
                    duration += data[i].daily_duration;
                    if (data[i].daily_activity == "Downtime") {
                        downTime = true;
                    }
                }

                var b = 24 - duration;
                if (!downTime && duration < 24) {
                    var c = confirm("Activity \"Downtime\" tidak ditemukan. Apakah Anda ingin menambahkan waktu yang tersisa (" + b + " jam) untuk \"Downtime\"?");
                    if (c) {
                        $("#dailyForm input[name='daily_duration']").val(b);
                        $("#daily_activity").val("Downtime");
                        $("#daily_unit").val("Unit Kosong");
                    }
                } else if (!downTime && duration > 24) {
                    alert("Activity Downtime tidak ditemukan. Dan durasi Anda melebihi 24jam")
                } else if (downTime && duration < 24) {
                    alert("Durasi yang ada kurang dari 24jam. Silahkan periksa kembali data Anda!");
                } else if (downTime && duration > 24) {
                    alert("Durasi yang ada melebihi 24jam. Silahkan periksa kembali data Anda!");
                } else {
                    saveDaily.apply();
                }
            }
        }
    })
    $("#monthlyPanel form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/filter/monthly", data)
        .done(function (res) {
            $("#monthlyView").html(res);
        })
        e.preventDefault();
    })
    var dailyCancel = function () {
        dailyTable.rows('.selected').deselect();
        $("#btnEditGroup").hide();
        $("#btnSaveGroup").show();
        $("#dailyForm input[name='daily_duration']").val(null);
        $("#dailyForm input[name='action']").val("create");
    }
    $("#btnCancelDaily").click(function () {
        dailyCancel.apply();
    })

    $("#accordion").on('hide.bs.collapse', function () {
        $("#accordion h4 i").removeClass("glyphicon-chevron-down");
        $("#accordion h4 i").addClass("glyphicon-chevron-up");
    })
    $("#accordion").on('show.bs.collapse', function () {
        $("#accordion h4 i").removeClass("glyphicon-chevron-up");
        $("#accordion h4 i").addClass("glyphicon-chevron-down");
    })
})