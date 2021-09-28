-- Cardinal Deus, Orfist

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	elseif n == 2 then
		return a.OnACT, p.HasPrompt, p.CB, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAbyssalDarkNight() then
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
		if obj.IsPlayerTurn() and (obj.IsAbyssalDarkNight() or obj.IsDarkNight()) then
			obj.SetAbilityPower(1, 5000)
		end
	elseif n == 2 then
		local t 
		for i=3,1,-1 do
			t = obj.SelectOption("Summon token.", "Don't summon token.")
			if t == 1 then
				obj.CallToken("dbt01_t01")
			else
				break
			end
		end
	end
	return 0
end