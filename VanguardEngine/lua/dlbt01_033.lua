-- Earnescorrect Member, Katalyn

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.This, q.Other, o.Resting, q.Count, 1
	elseif n == 3 then
		return q.Location, l.PlayerVC, q.Name, "Earnescorrect Leader, Clarissa", q.Other, o.Attacking, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttackHits, p.HasPrompt, p.CB, 1, p.Discard, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.Exists(3) then 
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
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Stand(1)
	end
	return 0
end