function CreateStudent(){
    var url = 'api/create';
    var student = {};
    student.name = $('#txtStudentName').val();
    student.fathername = $('#txtFatherName').val();
    student.address = $('#txtAddress').val();
    student.courses = $('#txtCourses').val();

    if($('#txtStudentName').val() === '' || $('#txtFatherName').val() === '' || $('#txtAddress').val() === '' 
    || $('#txtCourses').val()){
        alert("Feilds cannot be empty!");
    }


}