import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PatientModel } from '../../models/patient.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { FormsModule, NgForm } from '@angular/forms';
import { PatientPipe } from '../../pipe/patient.pipe';
import { FormValidateDirective } from 'form-validate-angular';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [CommonModule,PatientPipe,FormsModule,FormValidateDirective,RouterLink],
  templateUrl: './patients.component.html',
  styleUrl: './patients.component.css'
})
export class PatientsComponent implements OnInit{
  patients:PatientModel[]=[];

  @ViewChild("addModelCloseBtn") addModelCloseBtn:ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModelCloseBtn") updateModelCloseBtn:ElementRef<HTMLButtonElement> | undefined

  createModel:PatientModel=new PatientModel();
  updateModel:PatientModel=new PatientModel();

  search:string="";

  constructor(
    private http:HttpService,
    private toast:SwalService,
  ){}

  ngOnInit(): void {
    this.getAll();
  }

  getAll(){
    this.http.post<PatientModel[]>("Patient/GetAll",{},(res)=>{
      this.patients = res.data;
    });
  }

  add(form:NgForm){
    if(form.valid){
      this.http.post<string>("Patient/Create",this.createModel,(res)=>{
        this.toast.callToast(res.data,'success');
        this.getAll();
        this.addModelCloseBtn?.nativeElement.click();
        this.createModel = new PatientModel();
      })
    }
  }

  delete(id:string,fullName:string){
    this.toast.callSwall("Delete Patient",`Do you want to delete the patient : ${fullName}`,()=>{
      this.http.post<string>("Patient/DeleteById",{id:id},(res)=>{
        this.toast.callToast(res.data,"info");
        this.getAll();
      })
    })
  }

  openUpdatePage(patient:PatientModel){
    this.updateModel={...patient};
  }

  update(form:NgForm){
    this.http.post<string>("Patient/UpdateById",this.updateModel,(res)=>{
      this.toast.callToast(res.data,'success');
      this.getAll();
      this.updateModelCloseBtn?.nativeElement.click();
      this.updateModel = new PatientModel();
    })
  }
}
