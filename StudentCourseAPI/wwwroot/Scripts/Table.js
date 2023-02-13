
$(document).ready(function () {
    GetCourses();
});

function GetCourses() {
    var url = 'api/Courses/GetCourseWithStudents';

    $.ajax({
        url: url,
        contentType: "application/json ; charset = utf-8",
        dataType: "json",
        type: "Get",
        success: function (result) {
            if (result) {

                var options1 = '';
                debugger;
                for (var i = 0; i < result.length; i++) {
                    options1 += "<option value=" + result[i].id + " name=" + result[i].name + ">" + result[i].name + "</option>";
                }

               $("#MultipeSelect").html(options1);
                $("#MultipeSelect").selectpicker('refresh');



                
            }
        },
        error: function (msg) {
            alert(msg);
        }


    })

}




