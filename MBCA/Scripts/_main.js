var editor,
    dailyTable,
    path = '',
    //path = window.location.pathname,
    monthlyTable;

(function () {

    dailyEditor = new $.fn.dataTable.Editor({
        ajax: path + "/api/daily",
        table: "#dailyTable",
        fields: [
            //{ label: "Date", name: "daily_date", type: "datetime", format: "MM/DD/YYYY" },
            //{ label: "Vessel", name: "daily_vessel", type: "select" },
            //{ label: "Activity", name: "daily_activity", type: "select" },
            { label: "User Unit", name: "user_unit", type: "select" },
            { label: "Duration", name: "daily_duration" }
        ]
    }).on('preOpen', function () {
        dailyEditor.title('Add new Daily Activity');
    })

    dailyTable = $("#dailyTable").DataTable({
        dom: '<"dailyButton"B<"floatright">>rt',
        ajax: {
            url: path + "/api/daily",
            method: "post"
        },
        serverSide: true,
        columns: [
            //{ data: 'daily_date' },
            { data: 'user_unit' },
            //{ data: "daily_activity" },
            { data: "duration" }
        ],
        select: true,
        buttons: [
            {
                text: "Edit",
                action: function (e, dt, node, config) {
                    var a = dailyTable.rows('.selected').indexes()
                    if (a.length !== 0) {
                        var b = dailyTable.row(a).data();
                        console.log(b);
                        $('#inputDaily').collapse('show');
                        $("#daily_unit").val(b.user_unit);
                        //$("#daily_activity").val(b.daily_activity);
                        //$("#dailyForm input[name='daily_date']").val(b.daily_date);
                        $("#dailyForm input[name='time_at_dur']").val(b.duration);
                        $("#dailyForm input[name='action']").val("update");
                        $("#dailyForm input[name='id']").val(b.id);
                        //$("#dailyForm input[name='daily_fuel']").val(b.daily_fuel);
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
        buttons: [
            {
                extend: "collection",
                text: "Export to ..",
                buttons: ['excel']
            }
        ]
    })

    dailylogTable = $("#dailyLogTable").DataTable({
        dom: 'B<"floatright">rtip',
        ajax: {
            url: path + "/api/daily_log",
            type : "post"
        },
        columns: [
            { data: "tgl" },
            { data: "vessel" },
            { data: "fuel_tot" },
            { data: "user_unit" },
            { data: "duration" },
        ],
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

    $("#timeatForm").submit(function (e) {
        var data = $(this).serialize();
            $.post(path + "/api/cs/daily", data, function (res) {
                if (res === "success") {
                    dailyTable.ajax.reload();
                    $("#timeatForm input[name='time_at_dur']").val(null);
                    dailyCancel.apply();
                } else {
                    alert(res);
                }
            })
        e.preventDefault();
    })

    $("#daily_tgl").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date()
    });
    $("#drill_date").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date()
    });
    $("#monthlyPanel input").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date()
    });
    $('#t_start').timepicker({
        minuteStep: 1,
        showMeridian: false
    });
    $('#t_end').timepicker({
        minuteStep: 1,
        showMeridian: false 
    });


    $("#btnSaveDailyAct").click(function () {
        //alert("clik button save akticiti bro");
        var isi_form = $("#activityForm").serialize();
        var saveDaily = function () {
            $.post(path + "/api/save/daily", isi_form, function (res) {
                //console.log($("#activityForm")[0]);
                $("#activityForm")[0].reset();
                dailyTable.ajax.reload();
                dailylogTable.ajax.reload();
            })
        }
        var data = dailyTable.data();
        //console.log(data);

        if (data.length == 0) {
            alert("Belum ada data Durasi Time At, Periksa dulu input Time At");
        } else {
            var a = confirm("Apakah Anda yakin ingin menyimpan Data ini?");
            if (a) {
                var duration = 0;

                for (var i = 0; i < data.length; i++) {
                    duration += data[i].duration;
                    //if (data[i].daily_activity == "Downtime") {
                    //    downTime = true;
                    //}
                }
                var stb = ($("#activityForm input[name='standby']").val() === "") ? 0 : $("#activityForm input[name='standby']").val();
                var lod = ($("#activityForm input[name='load']").val() === "") ? 0 : $("#activityForm input[name='load']").val();
                var stm = ($("#activityForm input[name='steaming']").val() === "") ? 0 : $("#activityForm input[name='steaming']").val();
                var dtm = ($("#activityForm input[name='downtime']").val() === "") ? 0 : $("#activityForm input[name='downtime']").val();
                //console.log(stb,lod,stm,dtm);

                duration += parseFloat(stb) + parseFloat(lod) + parseFloat(stm) + parseFloat(dtm);
                //console.log(duration);
                var b = 24 - duration;
                
                //var fuel = parseInt($("#activityForm input[name='downtime']").val());

                if ($("#activityForm input[name='daily_fuel']").val() === "") {
                    alert("Total Fuel belum di Input")
                }
                else
                {
                    if (dtm <= 0 && duration < 24) {
                        var c = confirm("Durasi Aktivitas kurang dari 24 jam, apakah " + b + " jam akan ditambahkan ke Downtime?");
                        if (c) {
                            $("#activityForm input[name='downtime']").val(b);
                        }
                    } else if (dtm <= 0 && duration > 24) {
                        alert("Durasi Aktivitas melebihi 24 Jam, Mohon di periksa kembali!")
                    } else if (dtm >= 0 && duration < 24) {
                        alert("Durasi Aktivitas kurang 24 Jam, Mohon di periksa kembali!");
                    } else if (dtm >= 0 && duration > 24) {
                        alert("Durasi Aktivitas melebihi 24 Jam, Mohon di periksa kembali!")
                    } else {
                        saveDaily.apply();
                    }
                }
            }
        }
    });


    $("#btnSaveDataDaily").click(function () {
        var data = dailyTable.data();
        console.log(data);

        var saveDaily = function () {
            $.post(path + "/api/save/daily")
            .success(function (res) {
                if (res) {
                    monthlyTable.ajax.reload();
                    dailyTable.ajax.reload();
                } else {
                    alert(res);
                }
            })
        }

        if (data.length == 0) {
            alert("apa yang mau disimpen?");
        } else {
            var a = confirm("Apakah Anda yakin ingin menyimpan Data ini?");
            if (a) {
                var duration = 0,
                    downTime = false;

                for (var i = 0; i < data.length; i++) {
                    duration += data[i].duration;
                    //if (data[i].daily_activity == "Downtime") {
                    //    downTime = true;
                    //}
                }
                var stb = ($("#dailyForm input[name='standby']").val() === "") ? 0 : $("#dailyForm input[name='standby']").val();
                var lod = ($("#dailyForm input[name='load']").val() === "") ? 0 : $("#dailyForm input[name='load']").val();
                var stm = ($("#dailyForm input[name='steaming']").val() === "") ? 0 : $("#dailyForm input[name='steaming']").val();
                var dtm = ($("#dailyForm input[name='downtime']").val() === "") ? 0 : $("#dailyForm input[name='downtime']").val();
                //console.log(stb,lod,stm,dtm);
                
                duration += parseFloat(stb) + parseFloat(lod) + parseFloat(stm) + parseFloat(dtm);
                console.log(duration);
                var b = 24 - duration;
                if (dtm <= 0 && duration < 24) {
                    var c = confirm("Durasi Aktivitas kurang dari 24 jam, apakah "+b+" jam akan ditambahkan ke Downtime?");
                    if (c) {
                        $("#dailyForm input[name='downtime']").val(b);
                    }
                } else if (dtm <=0 && duration > 24) {
                    alert("Durasi Aktivitas melebihi 24 Jam, Mohon di periksa kembali!")
                } else if (dtm >=0 && duration < 24) {
                    alert("Durasi Aktivitas kurang 24 Jam, Mohon di periksa kembali!");
                } else if (dtm >= 0 && duration > 24) {
                    alert("Durasi Aktivitas melebihi 24 Jam, Mohon di periksa kembali!")
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