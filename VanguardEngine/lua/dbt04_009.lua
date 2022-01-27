-- グラビディア・バクビリート

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetCost("OnPlaceCost")
	ability1.SetCanFullyResolve("OnPlaceCanFullyResolve")
	ability1.SetActivation("OnPlace")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.RC)
	ability2.SetActivation("Cont")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlaceCost(check)
	if check then return obj.CanSB(1) end
	obj.SoulBlast(1)
end

function OnPlaceCanFullyResolve()
	return obj.Exists({q.Location, l.Drop, q.Other, o.Meteorite})
end

function OnPlace()
	obj.ChoosePutIntoOrderZone({q.Location, l.Drop, q.Other, o.Meteorite, q.Count, 1})
end

function Cont()
	if obj.IsPlayerTurn() then
		if obj.EnemyRCRetiredThisTurn() >= 1 then
			obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.Continuous)
		end
		if obj.EnemyRCRetiredThisTurn() >= 3 then
			obj.AddCardValue({q.Other, o.This}, cs.BonusCritical, 1, p.Continuous)
		end
	end
end