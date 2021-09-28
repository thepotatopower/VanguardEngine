-- Stealth Dragon, Togachirashi

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnDiscard, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastDiscarded() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.AddToSoul(1)
	end
	return 0
end
