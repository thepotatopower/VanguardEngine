-- Rancor Chain

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Other, o.Order, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerHand, q.Count, 2
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.OncePerTurn, p.SB, 1
	elseif n == 2 then
		return a.Cont, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() then
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
		local t = false
		obj.Draw(2)
		if obj.Exists(1) then
			t = obj.YesNo("Discard an order card?")
			if t then
				obj.SpecificDiscard(1)
			end
		end
		if not t then
			obj.Discard(2) 
		end
	elseif n == 2 then
		if obj.OrderPlayed() then
			obj.SetAbilityPower(3, 2000)
		end
	end
	return 0
end