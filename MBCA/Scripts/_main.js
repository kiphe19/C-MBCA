var editor,
    dailyTable,
    path = window.location.pathname,
    monthlyTable;

function getAV() {
    $.post(path + "/api/dataa")
            .success(function (e) {
                var activity = new Array();
                for (var i in e.data) {
                    var qp = {
                        label: e.data[i].activity_name,
                        value: e.data[i].activity_name
                    }
                    activity.push(qp);
                }
                dailyEditor.field("daily_activity").update(activity);
                monthlyEditor.field("monthly_activity").update(activity);
            })
    $.post(path + "/api/datav")
            .success(function (e) {
                var vessel = new Array();
                for (var i in e.data) {
                    var qp = {
                        label: e.data[i].name,
                        value: e.data[i].name
                    }
                    vessel.push(qp);
                }
                dailyEditor.field("daily_vessel").update(vessel);
                monthlyEditor.field("monthly_vessel").update(vessel);
            })
    $.post(path + "/api/datau")
           .success(function (e) {
               var unit = new Array();
               for (var i in e.data) {
                   var qp = {
                       label: e.data[i].unit_name,
                       value: e.data[i].unit_name
                   }
                   unit.push(qp);
               }
               dailyEditor.field("daily_unit").update(unit);
               monthlyEditor.field("monthly_unit").update(unit);
           })
}

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
        dom: '<"dailyButton"B<"floatright"f>>rtip',
        ajax: {
            url: path + "/api/daily",
            method: "post"
        },
        serverSide: true,
        deferRender: true,
        columns: [
            { data: 'daily_unit' },
            { data: "daily_activity" },
            { data: "daily_duration" }
        ],
        select: true,
        buttons: [
            //{ extend: "edit", editor: dailyEditor },
            {
                text: "Edit",
                action: function (e, dt, node, config) {
                    var a = dailyTable.rows('.selected').indexes()
                    if (a.length !==0) {
                        var b = dailyTable.row(a).data();
                        $("#daily_unit").val(b.daily_unit);
                        $("#daily_activity").val(b.daily_activity);
                        $("#dailyForm input[name='daily_duration']").val(b.daily_duration);
                        $("#dailyForm input[name='daily_type']").val(b.id);
                        $("#dailyForm input[name='daily_fuel']").val(b.daily_fuel);
                        $("#dailyForm button[type='submit']").hide();
                        $("#btnSaveDataDaily").hide();
                        $("#btnUpdateDaily").show();
                        $("#btnCancelDaily").show();
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
        dom: 'B<"floatright"f>rtip',
        ajax: {
            url: path + "/api/monthly",
            type: "post"
        },
        serverSide: true,
        columns: [
            { data: "monthly_date" },
            { data: "monthly_vessel" },
            { data: "monthly_activity" },
            { data: "monthly_unit" },
            { data: "monthly_duration" }
        ],
        select: true,
        buttons: [
            { extend: "edit", editor: monthlyEditor },
            { extend: "remove", editor: monthlyEditor },
            {
                extend: "collection",
                text: "Export to ..",
                buttons: ['excel']
            }
        ]
    }).one('processing', function () {
        getAV();
    });
})(jQuery)

$(document).ready(function () {
    
    $("#dailyForm").submit(function (e) {
        var data = $(this).serialize();
        if ($("#dailyForm input[name='daily_type']") == "create") {
            $.post(path + "/api/cs/daily", data, function (res) {
                if (res === "success") {
                    dailyTable.ajax.reload();
                    $("#dailyForm input[name='daily_duration']").val(null);
                    $("#dailyForm input[name='daily_fuel']").val(null);
                } else {
                    alert(res);
                }
            })
        } else {
            $.post(path + "/api/cs/daily/up", data, function (res) {
                if (res === "success") {
                    dailyTable.ajax.reload();
                    $("#dailyForm input[name='daily_duration']").val(null);
                    $("#dailyForm input[name='daily_fuel']").val(null);
                    dailyCancel.apply();
                } else {
                    alert(res);
                }
            })
        }
        e.preventDefault();
    })

    $("#daily_date").datetimepicker({
        format: "DD/MM/YYYY",
        maxDate: new Date(),
        minDate: new Date()
    })
    $("#btnSaveDataDaily").click(function () {
        if (dailyTable.data().length == 0) {
            alert("apa yang mau disimpen?");
        } else {
            var q = confirm("Are you sure you want to save current data?");
            if (q) {
                $.post(path + "/api/save/daily")
                    .success(function (res) {
                        if (res) {
                            monthlyTable.ajax.reload();
                            dailyTable.ajax.reload();
                        }
                    })
            }
        }
    })
    var dailyCancel = function () {
        dailyTable.rows('.selected').deselect();
        $("#btnCancelDaily").hide();
        $("#btnUpdateDaily").hide();
        $("#btnSaveDaily").show();
        $("#btnSaveDataDaily").show();
        $("#dailyForm input[name='daily_duration']").val(null);
        $("#dailyForm input[name='daily_fuel']").val(null);
    }
    $("#btnCancelDaily").click(function () {
        dailyCancel.apply();
    })
})