var std_id = '';
var coursesids = '';

$(document).ready(function () {
    //debugger;
    GetCourses();
    $('.selectpicker').selectpicker('refresh');
    ShowStudent();
    std_id = JSON.parse(localStorage.getItem('studentId'));
    if (std_id != null) {
        EditStudent(std_id);
       
    }

    $('#updatestd').click(function (e) {
        debugger;
        e.preventDefault();
        if (std_id != '') {
            UpdateStudent(std_id);
           /* clearform();*/

        }
        else {
            alert("NO ID!!");
        }

    });
    $('#refresh').on('click', function () {
        ShowStudent();
    });
});

function CreateStudent(courseIds) {
    debugger;
    courseIds = getSelected();

    var student = {};

    if ($('#txtName').val() === '' || $('#txtFatherName').val() === '' || $('#txtAddress').val() === '') {
        alert("Feilds cannot be empty!");
    } else {
        student.Name = $('#txtName').val();
        student.FatherName = $('#txtFatherName').val();
        student.Address = $('#txtAddress').val();
        student.courses = courseIds;
        debugger;

        if (student) {
            var url = 'api/Students/AddStudentWithCourses?courseIds=' + courseIds[0];
            for (var i = 1; i < courseIds.length; i++) {
                url += '&courseIds=' + courseIds[i];
            } 
            $.ajax({
                url,
                contentType: "application/json ; charset = utf-8",
                dataType: "json",
                data: JSON.stringify(student),
                type: "Post",
                success: function (result) {
                    console.log(result);                   
                },
                error: function (msg) {
                    console.error(msg);
                }
            });
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
                var rows = "";
                for (var i = 0; i < result.length; i++) {
                    
                    rows += `<tr>
                                <td>${result[i].id}</td>
                                <td>${result[i].name}</td>
                                <td>${result[i].fatherName}</td>
                                <td>${result[i].address}</td>
                                <td>${result[i].course}</td>
                                <td>
                                    <button class="btn btn-primary" id="edit" onClick="EditStudentP(${result[i].id})">Edit</button>
                                    &nbsp&nbsp&nbsp&nbsp
                                    <button class="btn btn-danger" onClick="DeleteStudent(${result[i].id})">Delete</button>
                                </td>
                            </tr>`;
                }
                $("#tblstd").html(rows);
            }
        },
        error: function (msg) {
            alert(msg);
        }
    })
}

function EditStudent(i) {
    var url = 'api/Students/GetStudentsWithCoursesbyID?id=' + i;
    $.ajax({
        url: url,
        contentType: "application/json ; charset = utf-8",
        dataType: "json",
        type: "Get",
        success: function (result) {
            if (result) {
                debugger;
                $("#edittbl").html('');
                var row = '';
                row += '<tr><td>' + result.id + '</td></ tr>';
                $("#edittbl").append(row);
                $('#txtID').val(result.id);
                $('#txtName').val(result.name);
                $('#txtFatherName').val(result.fatherName);
                $('#txtAddress').val(result.address);

                var selectedCourses = [];
                for (var i = 0; i < result.course.length; i++) {
                    var courseId = result.course[i].id;
                    var courseName = result.course[i].name;

                    $("#courseSelector option[value='" + courseId + "']").prop("selected", true).html(courseName);
                    selectedCourses.push(courseId);
                }
                // Re-render the dropdown
                $('.selectpicker').selectpicker('refresh');
            }
        },
        error: function (msg) {
            alert(JSON.stringify(msg));
        }
    });
}

function EditStudentP(studentId) {
   // debugger;
  //  alert(studentId);
    localStorage.setItem('studentId', studentId);
    window.location.href = "https://localhost:7183/edit.html";
    
}



function UpdateStudent(i) {
    let courses_id = getSelected();
    let url = 'api/Students/UpdateStudentWithCourses?id=' + i ;
    let student = {};
    student.Student = {
        Id: i,
        Name: $('#txtName').val(),
        FatherName: $('#txtFatherName').val(),
        Address: $('#txtAddress').val()
    };
    student.CourseIds = courses_id;

    $.ajax({
        url,
        contentType: "application/json ; charset = utf-8",
        dataType: "json",
        data: JSON.stringify(student),
        type: "Put",
        success: function () {

            localStorage.setItem("studentId",null);
            window.location.replace("https://localhost:7183/new.html");
           // $('#createstd').prop('disabled', false);
            // $('#updatestd').prop('disabled', true);

        },
        error: function () {
            alert("Error");
        }
    });
}

function DeleteStudent(i) {
    $.ajax({
        url: "api/Students/DeleteStudent?id=" + i,
        type: "DELETE",
        success: function (result) {
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
                var options = result.map(function (course) {
                    return "<option value=" + course.id + " name=" + course.name + ">" + course.name + "</option>";
                });
                $("#courseSelector").html(options);
                $('.selectpicker').selectpicker('refresh');
            }
        },
        error: function (msg) {
            alert(msg);
        }
    });
}


function clearForm() {
    $('#txtStudentName').val('');
    $('#txtFatherName').val('');
    $('#txtAddress').val('');
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


