import { FiInfo } from './../../models/FiInfo';
import { SharedService } from './../../service/shared.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { GlobalToastService } from 'src/app/components/toast/global-toast.service';
import { ApiRequest } from 'src/app/models/ApiRequest';
import { ApiService } from 'src/app/service/api.service';
import { SignalrHubService } from 'src/app/service/signalr-hub.service';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-game-page',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss']
})
export class GamePageComponent implements OnInit, OnDestroy {

  constructor(
    public _HttpClient: HttpClient,
    private _Router: Router,
    public _ApiService: ApiService,
    public _ToastService: GlobalToastService,
    private _SharedService: SharedService,
    public _Signalr: SignalrHubService,
    private modalService: NgbModal,
  ) {

  }

  ngOnInit(): void {
    this.InitServer();
    this.GetFiInfo();
    this.LoginToUserInfo();
    this.RoundStart();
    this.ReadTopUsers();
    this.ReLogin();

  }
  ngOnDestroy(): void {
    this._Signalr.DisConnect();
  }


  ShowToast(Msg: string, CssClass: string, Header: string) {
    this._ToastService.show(Msg, {
      className: CssClass,
      delay: 10000,
      HeaderTxt: Header,
    });
  }


  ReLogin() {
    this._Signalr.OnObservable("ReLogin").subscribe((Res: any) => {
      if (Res[0] == "此帳號已被從別處重複登入") {
        this._Signalr.DisConnect();
        alert(Res[0]);
        setTimeout(() => {
          localStorage.removeItem('UserId');
          localStorage.removeItem('Token');
          this._Router.navigateByUrl("/login");
        }, 5000);
      }
    });
  }



  Flag = true;
  WaitSeconds: any;
  RoundStart() {
    let Round = setInterval(() => {
      this.Time = new Date();
      let Second = this.Time.getSeconds();
      this.WaitSeconds = 60 - Second;
      if (Second % 60 == 0) {
        this.Flag = false;
        setInterval(() => { this.GameTimeRound(); }, 60000);
        clearInterval(Round);
      }
    }, 1000)

  }

  UserToken: any;
  Param: any;
  InitServer() {
    this.UserToken = localStorage.getItem("Token");
    this.Param = `token=${this.UserToken}`
    if (this.UserToken == null || undefined || "") {
      this.UserToken = localStorage.getItem("StrangerName");
      this.Param = `stranger=${this.UserToken}`
    }
    this._Signalr.Connect(`${this.Param}`);
  }


  OpenUserBoard: boolean = true;
  ToggleUserBoard() {
    this.OpenUserBoard = !this.OpenUserBoard;
  }


  OpenChatRoom: boolean = false;
  ToggleChatRoom() {
    this.OpenChatRoom = !this.OpenChatRoom;
  }

  OpenInfo: boolean = true;
  ToggleInfo() {
    this.OpenInfo = !this.OpenInfo;
  }

  OpenCard: boolean = true;
  ToggleCard() {
    this.OpenCard = !this.OpenCard;
  }
  IsLogin: boolean = false;
  UserData: any = { Name: "" };
  // UserName: any;
  UserId = localStorage.getItem('UserId');
  LoginToUserInfo() {

    if (this.UserId != "" && this.UserId != null) {
      this._SharedService.SharedData.subscribe((Res) => {
        this.UserData = Res;
      })
      this.IsLogin = true;
    }
    else {
      this.UserData.Name = localStorage.getItem("StrangerName");
    }
  }

  UserFiInfo = new FiInfo();
  GetFiInfo() {
    // Req.Args = this.UserId;
    // this._ApiService.GetFiInfo(Req).subscribe((Res) => {
    //   if (Res.Success) {
    //     this.UserFiInfo = Res.Data;
    //   }
    // });
    this._Signalr.OnObservable("ReadFiInfo").subscribe((Res: any) => {
      this.UserFiInfo = Res[0].Data;
      console.log(this.UserFiInfo, "userfi");
    });
  }



  GotUserBoard = false;
  UserBoard: any;
  ReadTopUsers() {
    this._Signalr.OnObservable("TopUserInBoard").subscribe((Res: any) => {
      this.UserBoard = Res[0].Data;
      this.GotUserBoard = true;
    });
  }


  Time = new Date();
  JoinRound = (this.Time.getHours() * 60) + this.Time.getMinutes();
  CurrentRound = (this.Time.getHours() * 60) + this.Time.getMinutes();
  GameTimeRound() {
    this.Time = new Date();
    this.CurrentRound = (this.Time.getHours() * 60) + this.Time.getMinutes();
    this.ToggleCard();
    setTimeout(() => {
      this.ToggleCard();
    }, 1800);
  }


  SaleAsset(Asset: any) {
    this._Signalr.Invoke("AssetSales", Asset);
    this.ShowToast("成功賣出資產", "bg-success text-light text-shadow", "錢董通知")
  }

  SaleLiabilities(Liabilities: any) {
    this._Signalr.Invoke("LiabilitieSales", Liabilities);
    this.ShowToast("成功還清負債", "bg-success text-light text-shadow", "錢董通知")

  }


  items: any = [];

  // 交易所
  @ViewChild('Modal', { static: true }) modalDOM: any;
  closeResult = '';
  open(content: any) {
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      ariaLabelledBy: 'modal-basic-title'
    }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
