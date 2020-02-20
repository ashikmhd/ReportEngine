function pageLoad() {
    $('.decimal_right').inputmask('decimal', { rightAlign: true });
    $('.decimal_left').inputmask('decimal', { rightAlign: false });

    $('.datepicker').datepicker({
        format: 'dd-mm-yyyy', separator: "/", assumeNearbyYear: true, calendarWeeks: true,
        autoclose: true, clearBtn: true, todayHighlight: true
    });
    $('.datepicker_ToCurrentDate').datepicker({
        format: 'dd-mm-yyyy', separator: "/", assumeNearbyYear: true, calendarWeeks: true,
        autoclose: true, clearBtn: true, todayHighlight: true, endDate: new Date()
    });
    $('.datepicker_FromCurrentDate').datepicker({
        format: 'dd-mm-yyyy', separator: "/", assumeNearbyYear: true, calendarWeeks: true,
        autoclose: true, clearBtn: true, todayHighlight: true, startDate: new Date()
    });

}