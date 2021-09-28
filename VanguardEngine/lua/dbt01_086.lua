-- Useful Recharger

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.GC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PutOnGC, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPutOnGC() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.IsDarkNight() or obj.IsAbyssalDarkNight() then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.IsDarkNight() then
			obj.AddTempShield(1, 5000)
		elseif obj.IsAbyssalDarkNight() then
			obj.AddTempShield(1, 10000)
		end
	end
	return 0
end