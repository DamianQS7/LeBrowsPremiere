$(document).ready(function () {
    GetService();
});

function OnAppointmentDateTimeChange(event) {
    var inputData = {
        'appointmentDate': $("#AppointmentDate").val(),
        'appointmentTime': $("#AppointmentTime").val(),
        'serviceId': $("#Appointment_ServiceId").val(),
        'appointmentId': $("#Appointment_Id").val()
    }

    var url = "/Customer/Appointment/ValidateAppointmentDateTime";
    $.get(url, inputData, function (resultData) {
        if (resultData.success) {
            toastr.success(resultData.message);
        }
        else {
            toastr.error(resultData.message);
        }
    });
}

function OnServiceChange(event) {

    GetService();
    OnAppointmentDateTimeChange(event);
}

function GetService() {
    var inputData = {
        'serviceId': $("#Appointment_ServiceId").val()
    }

    var url = "/Customer/Appointment/GetService";
    $.get(url, inputData, function (resultData) {
        if (resultData.success) {
            $("#ServiceDescription").text(resultData.serviceDescription);
            $("#ServicePrice").text(resultData.servicePrice);
            $("#ServiceDuration").text(resultData.serviceDuration);
        }
        else {
            toastr.error(resultData.message);
        }
    });
}
