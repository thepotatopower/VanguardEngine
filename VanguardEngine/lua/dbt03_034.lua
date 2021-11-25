-- デザイアデビル ボーショック

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetCost("OnRideCost")
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("OnRideTrigger")
	ability1.SetActivation("OnRide")
	-- cont
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.Cont)
	ability2.SetLocation(l.Soul)
	ability2.SetActivation("Cont")
end

function OnRideTrigger()
	return obj.Exists({q.SnapshotIndex, 0, q.Other, o.This}) and obj.Exists({q.SnapshotIndex, 1, q.Name, obj.GetNameFromCardID("dbt03_003")})
end

function OnRideCost(check)
	if check then return obj.CanAddToSoul({q.Location, l.PlayerHand, q.Count, 1}) end
	obj.ChooseAddToSoul({q.Location, l.PlayerHand, q.Count, 1})
end

function OnRide()
	obj.Search({q.Location, l.Deck, q.Name, obj.GetName({q.Other, o.This}), q.Count, 1})
end

function Cont()
	if obj.IsPlayerTurn() and obj.GetPlayerDamage() >= 4 then
		obj.AddCardValue({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt03_003")}, cs.BonusPower, 5000, p.Continuous)
	end
end