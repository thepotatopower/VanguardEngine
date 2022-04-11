-- Expanding World, Wilista

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Applicable, q.Other, o.Gem, q.Other, o.Player, q.Count, 1
	elseif n == 2 then
		return q.Location, l.GC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.Drop, q.Other, o.Gem
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrderPlayed, p.HasPrompt, p.IsMandatory, p.OncePerTurn
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsGuardian() then
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
		obj.Draw(1)
	elseif n == 2 then
		obj.SetAbilityShield(2, math.floor(obj.GetNumberOf(3) / 2) * 5000)
	end
	return 0
end