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
            { label: "Duration", name: "daily_duration" }
        ]
    }).one('preOpen', function () {
        getAV();
    }).on('preOpen', function () {
        dailyEditor.title('Add new Daily Activity');
    })

    dailyTable = $("#dailyTable").DataTable({
        dom: 'Bfrtip',
        ajax: {
            url: path + "/api/daily",
            method: "post"
        },
        serverSide: true,
        deferRender: true,
        columns: [
            { data: "daily_date" },
            { data: "daily_vessel" },
            { data: "daily_activity" },
            { data: "daily_duration" }
        ],
        select: true,
        buttons: [
            //{
            //    text: "Add new Daily Activity",
            //    action: function () {
            //        $("#dailyInput").modal({
            //            backdrop: false,
            //            show: true
            //        });
            //    }
            //},
            { extend: "create", editor: dailyEditor, text: "Add new Daily Activity" },
            { extend: "edit", editor: dailyEditor },
            { extend: "remove", editor: dailyEditor },
            {
                text: "Save data today",
                action: function (e, dt, node, config) {
                    if (dailyTable.data().length == 0) {
                        alert("apa yang mau disimpen?");
                    } else {
                        $.post(path + "/api/save/daily")
                        .success(function (e) {
                            monthlyTable.ajax.reaload();
                            dailyTable.ajax.reload();   
                        })
                    }
                }

            }
        ]
    });


    editor = new $.fn.dataTable.Editor({
        ajax: path + "/api/monthly",
        table: "#monthlyTable",
        fields: [
            { label: "Date", name: "monthly_date", type: "datetime", format: "MM/DD/YYYY" },
            { label: "Vessel", name: "monthly_vessel", type: "select" },
            { label: "Activity", name: "monthly_activity", type: "select" },
            { label: "Duration", name: "monthly_duration" }
        ]
    })

    monthlyTable = $("#monthlyTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: path + "/api/monthly",
            type: "post"
        },
        serverSide: true,
        columns: [
            { data: "monthly_date" },
            { data: "monthly_vessel" },
            { data: "monthly_activity" },
            { data: "monthly_duration" }
        ],
        select: true,
        buttons: [
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor },
            {
                extend: "collection",
                text: "Export to ..",
                buttons: [
                    'excel'
                ]
            }
        ]
    });

    $("#dailyForm").submit(function (e) {
        var data = $(this).serialize();
        $.ajax({
            url: path + "/api/cs/daily",
            type: "post",
            dataType: "json",
            data: data,
            success: function (e) {
                if (e) {
                    dailyTable.ajax.reload();
                    $("#dailyInput").modal('hide');
                    $("#dailyForm .form-control").each(function () {
                        $(this).val(null)
                    });
                } else {
                    $("#dailyInput .modal-footer").text(e)
                }
            }
        })
        e.preventDefault();
    })
})(jQuery)

$(document).ready(function () {
    $("#dailyDate").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date()
    })
})