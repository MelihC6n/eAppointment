import { Component, ElementRef, ViewChild } from '@angular/core';
import { departments } from '../../constants';
import { DoctorModel } from '../../models/doctor.model';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { DxSchedulerModule } from 'devextreme-angular';
import { HttpService } from '../../services/http.service';
import { AppointmentModel } from '../../models/appointment.model';
import { CreateAppointmentModel } from '../../models/create-appointment.model';
import { FormValidateDirective } from 'form-validate-angular';
import { PatientModel } from '../../models/patient.model';
import { SwalService } from '../../services/swal.service';

declare const $:any;

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule, DxSchedulerModule,FormValidateDirective],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  providers:[DatePipe]
})

export class HomeComponent {
  departments = departments;
  doctors: DoctorModel[] = [];

  createModel:CreateAppointmentModel= new CreateAppointmentModel();

  selectedDepartmentValue: number = 0;
  selectedDoctorId: string = '';

  @ViewChild("addModelCloseBtn") addModelCloseBtn: ElementRef<HTMLButtonElement> | undefined;

  constructor(
    private http: HttpService,
    private date: DatePipe,
    private swal: SwalService) {}

  appointments: AppointmentModel[] = [];

  getAllDoctors() {
    this.selectedDoctorId = '';
    if (this.selectedDepartmentValue > 0) {
      this.http.post<DoctorModel[]>(
        'Appointment/GetAllDoctorByDepartment',
        { departmentValue: +this.selectedDepartmentValue },
        (res) => {
          this.doctors = res.data;
        }
      );
    }
  }

  getAllAppointment() {
    if (this.selectedDoctorId) {
      this.http.post<AppointmentModel[]>(
        'Appointment/GetAllByDoctorId',
        { doctorId: this.selectedDoctorId },
        (res) => {
          this.appointments = res.data;
        }
      );
    }
  }

  onAppointmentFormOpening(e:any){
    e.cancel=true;

    this.createModel.startDate = this.date.transform(e.appointmentData.startDate,"dd.MM.yyyy HH:mm") ?? "";
    this.createModel.endDate = this.date.transform(e.appointmentData.endDate,"dd.MM.yyyy HH:mm") ?? "";
    this.createModel.doctorId = this.selectedDoctorId;

    $("#addModal").modal("show");
  }

  getPatient(){
    this.http.post<PatientModel>("Appointment/GetPatientByIdentityNumber",{identityNumber:this.createModel.identityNumber},
      (res)=>{
        if(res.data === null){
          this.createModel.patientId=null;
          this.createModel.firstName="";
          this.createModel.lastName="";
          this.createModel.city="";
          this.createModel.town="";
          this.createModel.fullAddress="";
        }

        this.createModel.patientId=res.data.id;
        this.createModel.firstName=res.data.firstName;
        this.createModel.lastName=res.data.lastName;
        this.createModel.city=res.data.city;
        this.createModel.town=res.data.town;
        this.createModel.fullAddress=res.data.fullAddress;
      }
    )
  }

  create(form:NgForm)
  {
    if(form.valid){
      this.http.post<string>("Appointment/Create",this.createModel,res=>{
        this.swal.callToast(res.data,"success");
        this.addModelCloseBtn?.nativeElement.click();
        this.createModel = new CreateAppointmentModel();
        this.getAllAppointment();
      })
    }
  }

  onAppointmentDeleting(e:any){
    e.cancel=true

    this.swal.callSwall("Delete Appointment!",`Do you want to delete ${e.appointmentData.patient.fullName}'s appointment? `,()=>{
      this.http.post<string>("Appointment/DeleteById",{id:e.appointmentData.id},res=>{
        this.swal.callToast(res.data);
        this.getAllAppointment();
      })
    });
  }

  onAppointmentDeleted(e:any){
    e.cancel=true
  }

  onAppointmentUpdating(e:any){
    e.cancel = true;

    const data = {
      id: e.oldData.id,
      startDate:this.date.transform(e.newData.startDate,"dd.MM.yyyy HH:mm"),
      endDate:this.date.transform(e.newData.endDate,"dd.MM.yyyy HH:mm")
    };

    this.http.post<string>("Appointment/Update",data,res=>{
      this.swal.callToast(res.data,"success");
      this.getAllAppointment();
    });
  }
}
