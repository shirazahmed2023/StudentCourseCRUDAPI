var std_id = '';
var coursesids = '';

$(document).ready(function () {
    GetCourses();
    ShowStudent();
    $('#updatestd').click(function () {

        if (std_id!='') {
            UpdateStudent(std_id);
            clearform();

        }
        else {
            alert("NO ID!!");
        }

    });
    $('#refresh').click(function () {

        ShowStudent();
    });  

  
});

function CreateStudent(courseIds) {
    debugger;
    courseIds = getSelected();

    var url = 'api/Students/AddStudentWithCourses?courseIds=' + courseIds[0];
    for (var i = 1; i < courseIds.length; i++) {
        url += '&courseIds=' + courseIds[i];
    } 

    

    var student = {};
    //|| $('#Cselect').val()) 
    if ($('#txtName').val() === '' || $('#txtFatherName').val() === '' || $('#txtAddress').val() === '') {
        alert("Feilds cannot be empty!");
    }
    else {
        //student.Id = id;
        student.Name = $('#txtName').val();
        student.FatherName = $('#txtFatherName').val();
        student.Address = $('#txtAddress').val();
        student.courses = courseIds;
        debugger;
        if (student) {
            $.ajax({
                url: url,
                contentType: "application/json ; charset = utf-8",
                dataType: "json",
                data: JSON.stringify(student),
                type: "Post",
                success: function (result) {
                   // alert(JSON.stringify(result));
                    clearform();
                    GetCourses();
                    ShowStudent();
                },
                error: function (msg) {
                    alert(JSON.stringify(msg));
                }


            })
        }
    }
}

function ShowStudent() {
    var url = 'api/Students/GetStudentsWithCourses';
    $.ajax({
        url: url,
        contentType: "application/json ; charset = utf-8",
        dataType: "json",
        type: "Get",
        success: function (result) {
            if (result) {
                $("#tblstd").html('');
                var row = '';
                for (var i = 0; i < result.length; i++) {
                   // debugger;
                   
                    row = row
                        + "<tr>"
                        + "<td>" + result[i].id + "</td>"
                        + "<td>" + result[i].name + "</td>"
                        + "<td>" + result[i].fatherName + "</td>"
                        + "<td>" + result[i].address + "</td>"
                        + "<td>" + result[i].course + "</td>"
                        + "<td><button class = 'btn btn-primary' id = 'edit' onClick = 'EditStudentP(" + result[i].id + ")'>Edit</button>&nbsp&nbsp&nbsp&nbsp<button class = 'btn btn-danger' onClick = 'DeleteStudent(" + result[i].id + ")'>Delete</button></td>"

                }
                if (row != '') {
                    $("#tblstd").append(row);
                }
            }
        },
        error: function (msg) {
            alert(msg);
        }
    })
}



function EditStudent(i) {
    var url = "api/Students/GetStudentsWithCoursesbyID?id=" + i;
    $.ajax({
        url: url,
        contentType: "application/json ; charset = utf-8",
        dataType: "json",
        type: "Get",
        success: function (result) {
            if (result) {

                std_id = result.id;
                $('#txtName').val(result.name);
                $('#txtFatherName').val(result.fatherName);
                $('#txtAddress').val(result.address);
                //debugger;
                // GetCourses();
                var selectedCourses = [];
                for (var i = 0; i < result.course.length; i++) {
                    var courseId = result.course[i].id;
                    var courseName = result.course[i].name;

                    $("#courseSelector option[value='" + courseId + "']").prop("selected", true).html(courseName);
                    selectedCourses.push(courseId);
                }
                // Re-render the dropdown
                $('.selectpicker').selectpicker('refresh');
                //alert(JSON.stringify(result));
            }
          //  $('#createstd').prop('disabled', true);
          //  $('#updatestd').prop('disabled', false);
        },
        error: function (msg) {
            alert(JSON.stringify(msg));
        }

    });

}

function EditStudentP(i) {
    localStorage.setItem("studentId", i);
    location.href = "https://localhost:7183/edit.html";
}

function UpdateStudent(i) {
    var courses_id = getSelected();
    var url = 'api/Students/UpdateStudentWithCourses?id=' + i;
    var student = {};
    
    var SName = $('#txtName').val();
    var SFatherName = $('#txtFatherName').val();
    var SAddress = $('#txtAddress').val();
    student.Student = {
        Id: i,
        Name: SName,
        FatherName: SFatherName,
        Address: SAddress
    };
    student.CourseIds = courses_id;
    if (student) { 
        $.ajax({
            url: url,
            contentType: "application/json ; charset = utf-8",
            dataType: "json",
            data: JSON.stringify(student),
            type: "Put",
            success: function (result) {
                $('#createstd').prop('disabled', false);
                $('#updatestd').prop('disabled', true);
                GetCourses();
                ShowStudent();
            },
            error: function (msg) {
                alert("Error");
            }
        });
       
    }
}



function DeleteStudent(i) {
    var url = "api/Students/DeleteStudent?id=" + i;
    $.ajax({
        url: url,
        contentType: "application/json ; charset = utf-8",
        dataType: "json",
        type: "Delete",
        success: function (result) {
            //alert(JSON.stringify(result));
            ShowStudent();
        },
        error: function (msg) {
            alert(JSON.stringify(msg));
        }

    });

}
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
                //debugger;
                for (var i = 0; i < result.length; i++) {
                    options1 += "<option value=" + result[i].id + " name=" + result[i].name +">" + result[i].name + "</option>";
                }
               // debugger;

                $("#courseSelector").html(options1);
              /*  setTimeout(() => {
                    $("#courseSelector").selectpicker('refresh');
                }, 15000); */
                
              /* $("#courseSelector").click(function () {
                    $("#courseSelector").selectpicker('refresh');    
                }); */
                
               // $("#MultipeSelect").html(options1);

                //$("#Cselect").multiselect().multiselectfilter();
               // $("#Cselect").append(options);
             
            }
        },
        error: function (msg) {
            alert(msg);
        }
    })
}

function clearform() {
    var student = {};
    student.name = $('#txtStudentName').val('');
    student.fathername = $('#txtFatherName').val('');
    student.address = $('#txtAddress').val('');
}

function getSelected() {
    var selectElem = document.getElementById("courseSelector");
    var selectedIds = [];
    for (var i = 0; i < selectElem.options.length; i++) {
        if (selectElem.options[i].selected) {
            selectedIds.push(selectElem.options[i].value);
        }
    }
    return selectedIds;
}


