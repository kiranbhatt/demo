import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from "@angular/forms";
import { ToastrService } from 'ngx-toastr';
import { RegisterUser } from '../_models/register';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  model: any = {};

  @Input() inputUsersFromHomeComponent: any;

  @Output() cancelRegisterUser = new EventEmitter<false>();

  // public means it can be accessed in Template also.
  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  registerForm = new UntypedFormGroup({

    username: new UntypedFormControl("", [Validators.required, Validators.email, Validators.pattern('')]),
    password: new UntypedFormControl("", [Validators.required, Validators.minLength(3), Validators.maxLength(10)])

  });


  ngOnInit(): void {
  }

  regsiter() {
    console.log(this.registerForm.value);
    let data = this.registerForm.value;

    let user: RegisterUser = {
      name: data.username,
      email: data.username,
      password: data.password,
      role: "Admin"
    };

    this.accountService.register(user).subscribe({

      next: response => {
        console.log(response);
        this.cancel();
      },
      error: error => this.toastr.error(error.error)
    });
  }

  cancel() {
    console.log("cancelled")
    this.model = {};

    this.cancelRegisterUser.emit(false);

  }

  get userValidation() {
    return this.registerForm.get("username");
  }

  get passwordValidation() {
    return this.registerForm.get("password");
  }

}
