-- 樹角獣 カリス

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetActivation("OnRideActivation")
	ability1.SetProperty(p.NotMandatory)
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.BackRowRC)
	ability2.SetActivation("Cont")
end

function OnRideTrigger()
	return obj.WasRodeUponBy(obj.GetNameFromCardID("dsd04_002"))
end

function OnRideActivation()
	obj.RevealFromDeck(1)
	obj.SuperiorCall({q.Location, l.Revealed, q.Grade, 2, q.Other, o.GradeOrLess, q.Other, o.Unit})
	obj.AddToSoul({q.Location, l.Revealed})
end

function Cont()
	if obj.IsAttackingUnit() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
	end
end