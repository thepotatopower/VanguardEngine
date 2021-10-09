-- Appassionato, Justine

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.This, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDiscard, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.Exists(1) and obj.IsPlayerTurn() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.CanSuperiorCallToSpecificCircle(1, FL.OpenCircle) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCallToSpecificCircle(1, FL.OpenCircle)
	end
	return 0
end