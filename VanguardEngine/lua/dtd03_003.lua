-- 駈出す決意 ユース

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.VC, l.RC)
	ability1.SetActivation("Cont")
	-- on ride
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnRide)
	ability2.SetTrigger("Trigger")
	ability2.SetCost("Cost")
	ability2.SetActivation("OnRide")
end

function Cont()
	if obj.IsAttackingUnit() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
	end
end

function Trigger()
	return obj.WasRodeUponBy(obj.LoadNameFromCardID("dtd03_002"))
end

function Cost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnRide()
	obj.LookAtTopOfDeck(3)
	obj.Select("Filter", {l.Looking}, 1, 0)
	if obj.Exists({q.Location, l.Selected, q.NameContains, obj.LoadName("Youthberk")}) then
		obj.Reveal({q.Location, l.Selected})
		obj.AddToHand({q.Location, l.Selected})
	elseif obj.Exists({q.Location, l.Selected, q.Grade, 2, q.Other, o.GradeOrLess, q.Other, o.Unit}) then
		obj.SuperiorCall({q.Location, l.Selected})
	end
	obj.RearrangeOnBottom({q.Location, l.Looking})
end

function Filter(id)
	return obj.NameContains(id, obj.LoadName("Youthberk")) or (obj.GradeOrLess(id, 2) and obj.IsUnit(id))
end