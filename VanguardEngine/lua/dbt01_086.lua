-- Useful Recharger

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.GC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.Damage, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.PlacedOnGC, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.LastPlacedOnGC() and obj.CanCB(2) and (obj.IsDarkNight() or obj.IsAbyssalDarkNight()) then
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

function Cost(n)
	if n == 1 then
		return obj.CounterBlast(2)
	end
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