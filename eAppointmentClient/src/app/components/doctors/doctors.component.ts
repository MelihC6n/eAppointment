import { Component, ElementRef, OnInit, ViewChild, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DoctorModel } from '../../models/doctor.model';
import { HttpService } from '../../services/http.service';
import { CommonModule } from '@angular/common';
import { departments } from '../../constants';
import { FormsModule, NgForm } from '@angular/forms';
import { FormValidateDirective } from 'form-validate-angular';
import { SwalService } from '../../services/swal.service';
import { DoctorPipe } from '../../pipe/doctor.pipe';

@Component({
  selector: 'app-doctors',
  standalone: true,
  imports: [CommonModule,RouterLink,FormsModule,FormValidateDirective,DoctorPipe],
  templateUrl: './doctors.component.html',
  styleUrl: './doctors.component.css'
})
export class DoctorsComponent implements OnInit {

  doctors:DoctorModel[]=[];
  departments=departments;

  @ViewChild("addModelCloseBtn") addModelCloseBtn:ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModelCloseBtn") updateModelCloseBtn:ElementRef<HTMLButtonElement> | undefined

  createModel:DoctorModel=new DoctorModel();
  updateModel:DoctorModel=new DoctorModel();

  search:string="";

  constructor(
    private http:HttpService,
    private toast:SwalService,
  ){}

  ngOnInit(): void {
    this.getAll();
  }

  getAll(){
    this.http.post<DoctorModel[]>("Doctor/GetAll",{},(res)=>{
      this.doctors = res.data;
    });
  }

  add(form:NgForm){
    if(form.valid){
      this.http.post<string>("Doctor/Create",this.createModel,(res)=>{
        this.toast.callToast(res.data,'success');
        this.getAll();
        this.addModelCloseBtn?.nativeElement.click();
        this.createModel = new DoctorModel();
      })
    }
  }

  delete(id:string,fullName:string){
    this.toast.callSwall("Delete Doctor",`Do you want to delete the doctor : ${fullName}`,()=>{
      this.http.post<string>("Doctor/DeleteById",{id:id},(res)=>{
        this.toast.callToast(res.data,"info");
        this.getAll();
      })
    })
  }

  openUpdatePage(doctor:DoctorModel){
    this.updateModel={...doctor};
    this.updateModel.departmentValue=doctor.department.value
  }

  update(form:NgForm){
    this.http.post<string>("Doctor/UpdateById",this.updateModel,(res)=>{
      this.toast.callToast(res.data,'success');
      this.getAll();
      this.updateModelCloseBtn?.nativeElement.click();
      this.updateModel = new DoctorModel();
    })
  }
}