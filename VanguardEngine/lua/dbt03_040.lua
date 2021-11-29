-- 柩機の獣 レイヨジア

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetActivation("OnPlace")
	-- ACT
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnACT)
	ability2.SetLocation(l.RC)
	ability2.SetCondition("ACTCondition")
	ability2.SetCost("ACTCost")
	ability2.SetActivation("ACT")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlace()
	obj.LookAtTopOfDeck(3)
	local added = obj.ChooseAddToHand({q.Location, l.Looking, q.Other, o.World, q.Count, 1, q.Min, 0})
	obj.Store(added)
	obj.Reveal({q.Location, l.Stored})
	obj.RearrangeOnBottom({q.Location, l.Looking})
end

function ACTCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_005")})
end

function ACTCost(check)
	if check then return obj.CanSB(1) and obj.CanRetire({q.Location, l.PlayerRC, q.Other, o.This}) end
	obj.SoulBlast(1)
	obj.Retire({q.Location, l.PlayerRC, q.Other, o.This})
end

function ACT()
	obj.CallToken("dbt01_t01")
end
