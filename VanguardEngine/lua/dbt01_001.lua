-- Vairina Valiente

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Grade, 3, q.Grade, 2, q.Grade, 1, q.Grade, 0, q.Other, o.OverDress
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1, 2
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	elseif n == 3 then
		return a.OnAttackHits, p.HasPrompt, p.OncePerTurn, p.CB, 1, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.InOverDress() then
			return true
		end
	elseif n == 3 then
		if obj.IsRearguard() and obj.IsAttackingUnit() and obj.InOverDress() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 2 then
		return true
	elseif n == 3 then
		return true
	end
	return false
end

function Activate(n)
	if n == 2 then
		obj.SetAbilityPower(3, obj.NumOriginalDress() * 5000)
	elseif n == 3 then
		obj.Stand(3)
	end
	return 0
end
