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
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnRide, t.Auto, p.HasPrompt
	elseif n == 2 then
		return a.Cont, t.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.WasRodeUponBy("Sylvan Horned Beast, Lattice") then
			return true
		end
	elseif n == 2 then
		if obj.IsBackRowRearguard() then
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

function Activate(n)
	if n == 1 then
		obj.RevealFromDeck(1)
		if obj.Exists(1) then
			obj.SuperiorCall(1)
		else
			obj.AddToSoul(2)
		end
	elseif n == 2 then
		if obj.IsAttackingUnit() then
			obj.SetAbilityPower(3, 5000)
		end
	end
	return 0
end