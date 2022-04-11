-- 宵闇月の輪舞曲 フェルティローザ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDriveCheck)
	ability1.SetLocation(l.VC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsPlayerTurn() and obj.Exists({q.Location, l.RevealedTrigger, q.Race, r.Ghost, q.UnitType, u.Normal, q.Other, o.Player})
end

function Condition()
	return obj.CanSuperiorCallToSpecificCircle({q.Location, l.RevealedTrigger, q.Race, r.Ghost, q.UnitType, u.Normal, q.Count, 1}, FL.FrontRow, FL.OpenCircle)
end

function Activation()
	obj.Store(obj.SuperiorCallToSpecificCircle({q.Location, l.RevealedTrigger, q.Race, r.Ghost, q.UnitType, u.Normal, q.Count, 1}, FL.FrontRow, FL.OpenCircle))
	if obj.Exists({q.Location, l.Stored}) and obj.CanPayCost("Cost") and obj.PayAdditionalCost() then
		obj.PayCost("Cost")
		obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusDrive, 1, p.UntilEndOfBattle)
	end
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end
