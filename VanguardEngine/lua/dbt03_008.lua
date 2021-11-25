-- 立志の魔法 カカロネ

function RegisterAbilities()
	-- on placed
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCondition("OnPlaceCondition")
	ability1.SetCost("OnPlaceCost")
	ability1.SetActivation("OnPlace")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.PlayerRC)
	ability2.SetActivation("Cont")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_008"), q.Name, obj.GetNameFromCardID("dbt01_046")})
end

function OnPlaceCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnPlace()
	obj.LookAtTopOfDeck(2)
	obj.RearrangeOnTop({q.Location, l.Looking})
end

function Cont()
	if obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_008")}) then
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 2000, p.Continuous)
		obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusShield, 5000, p.Continuous)
	end
end