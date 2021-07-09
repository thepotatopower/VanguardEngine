-- Sylvan Horned Beast, Charis

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Revealed, q.Grade, 0, q.Grade, 1, q.Grade, 2, q.Other, o.Unit, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Revealed
	elseif n == 3 then
		return q.Location, l.BackRow, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRodeUponThisTurn() and obj.VanguardIs("Sylvan Horned Beast, Lattice") then
			return true
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() and obj.Exists(3) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	elseif n == 2 then
		return true
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.RevealFromDeck(1)
		if obj.Exists(1) then
			obj.SuperiorCall(1)
		else
			obj.AutoAddToSoul(2)
		end
		obj.OnRideAbilityResolved()
	elseif n == 2 then
		obj.AddBattleOnlyPower(3, 5000)
	end
	return 0
end