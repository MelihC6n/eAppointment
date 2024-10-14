import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { FormValidateDirective } from 'form-validate-angular';
import { RouterLink } from '@angular/router';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { UserPipe } from '../../pipe/user.pipe';
import { UserModel } from '../../models/user.model';
import { RoleModel } from '../../models/role.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule,UserPipe,FormsModule,FormValidateDirective,RouterLink],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  users:UserModel[]=[];
  roles:RoleModel[]=[];


  @ViewChild("addModelCloseBtn") addModelCloseBtn:ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModelCloseBtn") updateModelCloseBtn:ElementRef<HTMLButtonElement> | undefined

  createModel:UserModel=new UserModel();
  updateModel:UserModel=new UserModel();

  search:string="";

  constructor(
    private http:HttpService,
    private toast:SwalService,
  ){}

  ngOnInit(): void {
    this.getAll();
  }

  getAll(){
    this.http.post<UserModel[]>("Users/GetAll",{},(res)=>{
      this.users = res.data;
    });
  }

  add(form:NgForm){
    if(form.valid){
      this.http.post<string>("Users/Create",this.createModel,(res)=>{
        this.toast.callToast(res.data,'success');
        this.getAll();
        this.addModelCloseBtn?.nativeElement.click();
        this.createModel = new UserModel();
      })
    }
  }

  delete(id:string,fullName:string){
    this.toast.callSwall("Delete User",`Do you want to delete the user : ${fullName}`,()=>{
      this.http.post<string>("Users/DeleteById",{id:id},(res)=>{
        this.toast.callToast(res.data,"info");
        this.getAll();
      })
    })
  }

  openUpdatePage(patient:UserModel){
    this.updateModel={...patient};
  }

  update(form:NgForm){
    this.http.post<string>("Users/UpdateById",this.updateModel,(res)=>{
      this.toast.callToast(res.data,'success');
      this.getAll();
      this.updateModelCloseBtn?.nativeElement.click();
      this.updateModel = new UserModel();
    })
  }
}
