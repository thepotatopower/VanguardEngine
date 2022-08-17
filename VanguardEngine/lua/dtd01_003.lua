-- 好音の芽吹き グラシア

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTriggerCondition("Trigger")
	ability1.SetActivationCondition("Condition")
	ability1.SetActivation("OnRide")
	ability1.SetProperty(p.NotMandatory)
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function Trigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dtd01_002"))
end

function Condition()
	return obj.CanSuperiorCall({q.Other, o.ThisFieldID})
end

function OnRide()
	obj.SuperiorCall({q.Other, o.ThisFieldID})
end

function Cont()
	if obj.IsBooster() and obj.Exists({q.Location, l.PlayerVC, q.Other, o.Attacking}) then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end