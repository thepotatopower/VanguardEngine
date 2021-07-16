-- Embodiment of Armor, Bahr

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Deck, q.Grade, 1, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, false, p.CB, 1
	elseif n == 2 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Dragon Knight, Nehalem") and obj.CanCB(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and not obj.Activated() and obj.VanguardIsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(2) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.Search(2)
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.AddTempPower(3, 5000)
	end
	return 0
end