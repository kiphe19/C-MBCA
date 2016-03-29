var editor,
    dailyTable,
    path = '',
    //path = window.location.pathname,
    monthlyTable;
var tgl = new Date();

$(document).ready(function () {
    $("#btnCancelDaily").hide();
    $("#btnUpdateDailyAct").hide();
    var dailyEditor = new $.fn.dataTable.Editor({
        ajax: path + "/api/daily/0",
        table: "#dailyTable",
        fields: [
            //{ label: "Date", name: "daily_date", type: "datetime", format: "MM/DD/YYYY" },
            //{ label: "Vessel", name: "daily_vessel", type: "select" },
            //{ label: "Activity", name: "daily_activity", type: "select" },
            //{ label: "User Unit", name: "user_unit", type: "select" },
            //{ label: "Duration", name: "daily_duration" }
        ]
    }).on('preOpen', function () {
        dailyEditor.title('Add new Daily Activity');
    })

    var dailyTable = $("#dailyTable").DataTable({
        dom: '<"dailyButton"B<"floatright">>rt',
        ajax: {
            url: path + "/api/daily/0",
            method: "post"
        },
        serverSide: true,
        columns: [
            { data: "unit" },
            { data: "dur" }
        ],
        select: true,
        buttons: [
            {
                extend: 'create',
                text: 'New',
                action: function () {
                    $("#timeatForm")[0].reset();
                    $("#timeatForm input[name='action']").val("create");
                    $("#timeatForm input[name='id']").val("");
                    $("#timeatForm button[type='submit']").text("Add");
                }
            },
            {
                extend:'edit',
                text: "Edit",
                action: function (e, dt, node, config) {
                    var a = dailyTable.rows('.selected').indexes();
                    if (a.length !== 0) {
                        var b = dailyTable.row(a).data();
                        //console.log(b);
                        $("#timeatForm input[name='action']").val("update");
                        $("#timeatForm input[name='id']").val(b.id_temp);
                        $("#timeatForm select[name = 'daily_unitid'] option[value=" + b.id_un + "]").prop("selected", true);
                        $("#timeatForm input[name='time_at_dur']").val(b.dur);
                        $("#timeatForm button[type='submit']").text("Update");
                    }
                }
            },
            { extend: "remove", editor: dailyEditor },
        ]
    });



    var yyyy = tgl.getFullYear();
    var mm = (tgl.getMonth() + 1);
    var dd = (tgl.getDate() < 10) ? "0" + tgl.getDate() : tgl.getDate();
    var tg1, tg2;
    if (dd <= 25) {
        tg1 = yyyy + "-" + (((mm - 1) < 10) ? "0" + (mm - 1) : (mm - 1)) + "-" + 25;
        tg2 = yyyy + "-" + ((mm < 10) ? "0" + mm : mm) + "-" + 25;
    }
    else {
        tg1 = yyyy + "-" + ((mm < 10) ? "0" + mm : mm) + "-" + 25;
        tg2 = yyyy + "-" + (((mm + 1) < 10) ? "0" + (mm + 1) : (mm + 1)) + "-" + 25;
    }
    console.log(tg1, tg2);
    var dailyLogEditor = new $.fn.dataTable.Editor({
            
        url: path + "/api/dailylog/" + tg1 + "/" + tg2,
        table: "#DailyLogTable",
        //fields: [{
        //    //label: "First name:",
        //    name: "daily_table.tgl"
        //}, {
        //    //label: "Last name:",
        //    name: "daily_table.id_vessel"
        //}, {
        //    //label: "Manager:",
        //    name: "daily_table.id",
        //    type: "select"
        //}
        //]
    });

    var dailylogTable = $("#DailyLogTable").DataTable({
        //dom: 'B<"floatright">rtip',
        dom: "Bfrtip",
        ajax: {
            url: path + "/api/dailylog/" + tg1 + "/" + tg2,
            type: "post"
        },
        select : true,
        columns: [
            { data: null },
            { data: "tg" },
            { data: "nm_ves" },
            { data: "stb" },
            { data: "ld" },
            { data: "stm" },
            { data: "dt" },
            { data: "fuel_t" }
        ],
        buttons: [
            {
                extend:'edit',
                text : 'Edit Activity',
                action : function(){
                    var a = dailylogTable.rows('.selected').indexes();
                    var b = dailylogTable.row(a).data();
                    console.log(a);
                    console.log(b);
                    $("#btnCancelDaily").show();
                    $("#activityForm input[name='action']").val("update");
                    $("#activityForm input[name='id']").val(b.id_dt);

                    $("#activityForm input[name='daily_date']").val(b.tg);
                    $("#activityForm select[name = 'daily_vesselid'] option[value=" + b.id_ves+ "]").prop("selected", true);
                    $("#activityForm input[name='daily_fuel']").val(b.fuel_t);
                    $("#activityForm input[name='standby']").val(b.stb);
                    $("#activityForm input[name='load']").val(b.ld);
                    $("#activityForm input[name='steaming']").val(b.stm);
                    $("#activityForm input[name='downtime']").val(b.dt);
                    (b.mob == 1) ? $("#activityForm input[name='mob']").prop("checked", true) : $("#activityForm input[name='mob']").prop("checked", false);
                    $("#btnUpdateDailyAct").show();
                    $("#btnSaveDailyAct").hide();

                    //ambilUnitDailyVes(b.id_dt);
                    dailyTable.ajax.url('/api/daily/' + b.id_dt).load();

                        
                }
            },
            { extend: 'remove', text: 'Delete', editor: dailyLogEditor }
            //{
            //    extend: "remove", text: "Delete Unit Daily", editor: dailyLogEditor
            //}
            //{
            //    extend: "collection",
            //    text: "Export to ..",
            //    buttons: ['excel']
            //}
        ],
        fnDrawCallback: function (oSettings) {
            if (oSettings.bSorted || oSettings.bFiltered) {
                for (var i = 0, iLen = oSettings.aiDisplay.length ; i < iLen ; i++) {
                    $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html(i + 1);
                }
            }
        }
    });

    //var ambilUnitDailyVes = ()
    //var ambilUnitDailyVes = $("#DailyLogTable").DataTable({
    //var ambilUnitDailyVes = $("#dailyTable").DataTable({
    //    dom: '<"dailyButton"B<"floatright">>rt',
    //    ajax: {
    //        url: path + "/api/daily",
    //        method: "post"
    //    },
    function ambilUnitDailyVes(a){
        $("#dailyTable").DataTable({
            dom: '<"dailyButton"B<"floatright">>rt',
            ajax: {
                url: path + "/api/daily/"+a,
                method: "post"
            }
        })
    }
    

    drillEditor = new $.fn.dataTable.Editor({
        ajax: path + "/api/drill/" + tg1 + "/" + tg2 + "/0",
        table: "#drillTable"
    });
    drillcompTable = $("#drillTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: path + "/api/drill/" + tg1 + "/" + tg2 + "/0",
            method: "post"
        },
        columns: [
            { data: 'unit_table.name' },
            { data: "drilling_table.tgl" },
            { data: 'drilling_table.well' },
            { data: "drilling_table.afe" },
            { data: "drilling_table.psc_no" },
            { data: 'drilling_table.t_start' },
            { data: 'drilling_table.t_end' },
            { data: 'drilling_table.durasi' }

        ],
        select: true,
        buttons: [
            
            {
                extend: 'edit',
                text: "Edit Dril Completion",
                action: function (e, dt, node, config) {
                    var a = drillcompTable.rows('.selected').indexes();
                    var b = drillcompTable.row(a).data();
                    //console.log(a);
                    //console.log(b);
                    $("#drillForm option[value='" + b.drilling_table.id_unit + "']").prop("selected", true);
                    $("#drillForm input[name='drill_date']").val(b.drilling_table.tgl);
                    $("#drillForm input[name='well']").val(b.drilling_table.well);
                    $("#drillForm input[name='afe']").val(b.drilling_table.afe);
                    $("#drillForm input[name='psc']").val(b.drilling_table.psc_no);
                    $("#drillForm input[name='t_start']").val(b.drilling_table.t_start);
                    $("#drillForm input[name='t_end']").val(b.drilling_table.t_end);
                    $("#drillForm button[type='submit']").text("Edit D&C");
                    $("#drillForm input[name='action']").val("update");
                    $("#drillForm input[name='id']").val(b.drilling_table.id);
                }
            },
            { extend: 'remove', text: 'Delete Dril Completion', editor: drillEditor }
        ]
    });

    //$("#activityForm input[name='daily_date']").bind("input", function () {
    //    //$("#timeatForm input[name = 'date_timeat']").val($(this).val());
    //    alert($(this).val());
    //});

    //$("#daily_tgl").live("input",function () {
    //    alert("tara");
    //});
    $("#timeatForm").submit(function (e) {
        $("#timeatForm input[name = 'date_timeat']").val($("#activityForm input[name='daily_date']").val());
        var data = $(this).serialize();
        console.log(data);
        $.post(path + "/api/cs/daily", data, function (res) {
            if (res === "success") {
                //alert(res);
                dailyTable.ajax.reload();
                $("#timeatForm input[name='time_at_dur']").val(null);
                //dailyCancel.apply();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    });
    $("#drillForm").submit(function (e) {
        var data = $(this).serialize();
        console.log(data);
        $.post(path + "/api/save/drill", data, function (res) {
            //console.log(res);
            if (res === "success") {
                drillcompTable.ajax.reload();
                $("#drillForm input").val(null);
                $("#drillForm input[name='action']").val("create");
                $("#drillForm button[type='submit']").text("Simpan Drill");


            }
            else {
                alert(res);
            }
        });
        e.preventDefault();
    });

    $("#daily_tgl").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date(),
        
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

    //var today = new Date();
    //var today = new Date();
    var yyyy = tgl.getFullYear();
    //var mm = ((today.getMonth() + 1) < 10) ? "0" + (today.getMonth() + 1) : (today.getMonth() + 1);
    var mm = (tgl.getMonth() + 1);
    var dd = (tgl.getDate() < 10) ? "0" + tgl.getDate() : tgl.getDate();
    var tg11, tg21;
    if (dd <= 25) {
        tg11 = (((mm - 1) < 10) ? "0" + (mm - 1) : (mm - 1)) + "/" + 25 + "/" + yyyy;
        tg21 = ((mm < 10) ? "0" + mm : mm) + "/" + 25 + "/" + yyyy;
    }
    else {
        tg11 = ((mm < 10) ? "0" + mm : mm) + "/" + 25 + "/" + yyyy;
        tg21 = (((mm + 1) < 10) ? "0" + (mm + 1) : (mm + 1)) + "/" + 25 + "/" + yyyy;
    }


    $("#filterDrill input[name='fd_t_from']").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date(),
        //defaultDate : new Date("2018-03-02")
    });
    $("#filterDrill input[name='fd_t_from']").val(tg11);
    $("#filterDrill input[name='fd_t_to']").datetimepicker({
        format: "MM/DD/YYYY",
        maxDate: new Date()
    });
    //$("#filterDrill input[name='fd_t_to']").val(new Date(2011,02,20))
    $("#filterDrill input[name='fd_t_to']").val(tg21);


    $("#btnSaveDailyAct").click(function () {
        //alert("clik button save akticiti bro");
        var isi_form = $("#activityForm").serialize();
        console.log(isi_form);
        var saveDaily = function () {
            $.post(path + "/api/save/daily", isi_form, function (res) {
                //console.log($("#activityForm")[0]);
                $("#activityForm")[0].reset();
                //$("#activityForm input[name='action']").val("create");

                dailyTable.ajax.reload();
                dailylogTable.ajax.reload();

            })
        }
        var data = dailyTable.data();
        //console.log(data);
        //console.log(data.length);

        if (data.length == 0) {
            alert("Belum ada data Durasi Time At, Periksa dulu input Time At");
        } else {
            var a = confirm("Apakah Anda yakin ingin menyimpan Data ini?");
            if (a) {
                var duration = 0;

                for (var i = 0; i < data.length; i++) {
                    duration += data[i].temp_daily.duration;
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
                else {
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

    $("#drillcari").click(function () {
        var dari = new Date($("#filterDrill input[name = 'fd_t_from']").val());
        var ke = new Date($("#filterDrill input[name = 'fd_t_to']").val());
        var unit = $("#cb_unit").val();
        var dari1 = dari.getFullYear() + "-" + (dari.getMonth() + 1) + "-" + dari.getDate();
        var ke1 = ke.getFullYear() + "-" + (ke.getMonth() + 1) + "-" + ke.getDate();
        drillcompTable.ajax.url('api/drill/' + dari1 + '/' + ke1 + '/' + unit).load();
        //drillcompTable.ajax.url('api/drill').load();
    });

    //$("#btnSaveDataDaily").click(function () {
    //    var data = dailyTable.data();
    //    //console.log(data);

    //    var saveDaily = function () {
    //        $.post(path + "/api/save/daily")
    //        .success(function (res) {
    //            if (res) {
    //                monthlyTable.ajax.reload();
    //                dailyTable.ajax.reload();
    //            } else {
    //                alert(res);
    //            }
    //        })
    //    }

    //    if (data.length == 0) {
    //        alert("apa yang mau disimpen?");
    //    } else {
    //        var a = confirm("Apakah Anda yakin ingin menyimpan Data ini?");
    //        if (a) {
    //            var duration = 0,
    //                downTime = false;

    //            for (var i = 0; i < data.length; i++) {
    //                duration += data[i].duration;
    //                //if (data[i].daily_activity == "Downtime") {
    //                //    downTime = true;
    //                //}
    //            }
    //            var stb = ($("#dailyForm input[name='standby']").val() === "") ? 0 : $("#dailyForm input[name='standby']").val();
    //            var lod = ($("#dailyForm input[name='load']").val() === "") ? 0 : $("#dailyForm input[name='load']").val();
    //            var stm = ($("#dailyForm input[name='steaming']").val() === "") ? 0 : $("#dailyForm input[name='steaming']").val();
    //            var dtm = ($("#dailyForm input[name='downtime']").val() === "") ? 0 : $("#dailyForm input[name='downtime']").val();
    //            //console.log(stb,lod,stm,dtm);

    //            duration += parseFloat(stb) + parseFloat(lod) + parseFloat(stm) + parseFloat(dtm);
    //            //console.log(duration);
    //            var b = 24 - duration;
    //            if (dtm <= 0 && duration < 24) {
    //                var c = confirm("Durasi Aktivitas kurang dari 24 jam, apakah " + b + " jam akan ditambahkan ke Downtime?");
    //                if (c) {
    //                    $("#dailyForm input[name='downtime']").val(b);
    //                }
    //            } else if (dtm <= 0 && duration > 24) {
    //                alert("Durasi Aktivitas melebihi 24 Jam, Mohon di periksa kembali!")
    //            } else if (dtm >= 0 && duration < 24) {
    //                alert("Durasi Aktivitas kurang 24 Jam, Mohon di periksa kembali!");
    //            } else if (dtm >= 0 && duration > 24) {
    //                alert("Durasi Aktivitas melebihi 24 Jam, Mohon di periksa kembali!")
    //            } else {
    //                saveDaily.apply();
    //            }
    //        }
    //    }
    //})
    //$("#monthlyPanel form").submit(function (e) {
    //    var data = $(this).serialize();
    //    $.post("api/filter/monthly", data)
    //    .done(function (res) {
    //        $("#monthlyView").html(res);
    //    })
    //    e.preventDefault();
    //})
    //var dailyCancel = function () {
    //    dailyTable.rows('.selected').deselect();
    //    $("#btnEditGroup").hide();
    //    $("#btnSaveGroup").show();
    //    $("#dailyForm input[name='daily_duration']").val(null);
    //    $("#dailyForm input[name='action']").val("create");
    //}
    $("#btnCancelDaily").click(function () {
        //dailyCancel.apply();
        console.log("ini cancel di klik", $("#activityForm")[0]);
        $("#btnCancelDaily").hide();
        $("#btnSaveGroup").show();
        $("#btnUpdateDailyAct").hide();
        $("#btnSaveDailyAct").show();
        $("#activityForm")[0].reset();
        $("#activityForm input[name='action']").val("create");

        
    })
    $("#accordion").on('hide.bs.collapse', function () {
        $("#accordion h4 i").removeClass("glyphicon-chevron-down");
        $("#accordion h4 i").addClass("glyphicon-chevron-up");
    })
    $("#accordion").on('show.bs.collapse', function () {
        $("#accordion h4 i").removeClass("glyphicon-chevron-up");
        $("#accordion h4 i").addClass("glyphicon-chevron-down");
    })
});