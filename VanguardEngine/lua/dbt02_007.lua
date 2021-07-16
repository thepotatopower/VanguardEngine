-- Heavenly Bow of Edifying Guidance, Refuerzos

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.BackRow, q.Other, o.This, q.Other, o.Resting, q.Count, 1
	elseif n == 3 then
		return q.Location, l.LastStood, q.Other, o.NotThis, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 2 then
		return a.OnStand, t.Auto, p.HasPrompt, true, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
			return true
		end
	elseif n == 2 then
		if obj.Exists(2) and not obj.Activated() and obj.Exists(3) then
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
		if obj.PersonaRode() then
			obj.AddSkill(1, s.Boost)
		else
			obj.RemoveSkill(1, s.Boost)
		end
	elseif n == 2 then
		obj.Stand(2)
	end
	return 0
end
